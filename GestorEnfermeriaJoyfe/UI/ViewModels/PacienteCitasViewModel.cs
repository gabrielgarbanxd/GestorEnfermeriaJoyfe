using GestorEnfermeriaJoyfe.Adapters.CiteAdapters;
using GestorEnfermeriaJoyfe.Adapters.VisitAdapters;
using GestorEnfermeriaJoyfe.Domain.Cite;
using GestorEnfermeriaJoyfe.Domain.Patient;
using GestorEnfermeriaJoyfe.Domain.Patient.ValueObjects;
using GestorEnfermeriaJoyfe.Domain.Visit.ValueObjects;
using GestorEnfermeriaJoyfe.Domain.Visit;
using GestorEnfermeriaJoyfe.UI.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GestorEnfermeriaJoyfe.UI.ViewModels
{
    public class PacienteCitasViewModel : ViewModelBase
    {
        private readonly Patient _patient;
        private readonly CiteController _citeController;
        private readonly VisitController _visitController;

        private ObservableCollection<Cite> _cites;
        public ObservableCollection<Cite> Cites
        {
            get => _cites;
            set
            {
                _cites = value;
                OnPropertyChanged(nameof(Cites));
            }
        }

        private Cite _selectedCite;
        public Cite SelectedCite
        {
            get => _selectedCite;
            set
            {
                _selectedCite = value;
                OnPropertyChanged(nameof(SelectedCite));
            }
        }

        public ICommand GoBackCommand { get; }
        public ICommand CreateCiteCommand { get; }
        public ICommand EditCiteCommand { get; }
        public ICommand DeleteCiteCommand { get; }
        public ICommand AddVisitCommand { get; }

        public PacienteCitasViewModel(Patient patient, CiteController citeController, VisitController visitController)
        {
            _patient = patient;
            _citeController = citeController;
            _visitController = visitController;

            GoBackCommand = new ViewModelCommand((object parameter) => MainViewModelRouter.Instance.OnShowSinglePacienteView(patient));
            CreateCiteCommand = new ViewModelCommand(ExecuteCreateCiteCommand);
            EditCiteCommand = new ViewModelCommand(ExecuteEditCiteCommand);
            DeleteCiteCommand = new ViewModelCommand(ExecuteDeleteCiteCommand);
            AddVisitCommand = new ViewModelCommand(ExecuteAddVisitCommandAsync);

            LoadCites();
        }

        private async void ExecuteCreateCiteCommand(object parameter)
        {
            Citaform dialog = new();
            bool? result = dialog.ShowDialog();

            if (result == false) return;

            Cite newCite;

            try
            {
                newCite = Cite.FromPrimitives(0, _patient.Id.Value, dialog.txtNote.Text, null, dialog.dpFechaInicio.SelectedDate ?? DateTime.Now);
            }
            catch (Exception)
            {
                MessageBox.Show("Error al crear la cita");
                ExecuteCreateCiteCommand(parameter);
                return;
            }

            var response = await _citeController.Create(newCite);

            if (response.Success)
            {
                newCite.SetId(new (response.Id));
                Cites.Add(newCite);
                MessageBox.Show("Cita creada con éxito");
            }
            else
            {
                MessageBox.Show("Error al crear la cita");
            }


        }

        private async void ExecuteEditCiteCommand(object parameter)
        {
            if (SelectedCite == null)
            {
                MessageBox.Show("Seleccione una cita para editar");
                return;
            }

            int citeId = SelectedCite.Id.Value;
            int index = Cites.IndexOf(SelectedCite);

            Citaform dialog = new(SelectedCite);
            bool? result = dialog.ShowDialog();

            if (result == false) return;

            Cite updatedCite;

            try
            {
                updatedCite = Cite.FromPrimitives(citeId, _patient.Id.Value, dialog.txtNote.Text, null, dialog.dpFechaInicio.SelectedDate ?? DateTime.Now);
            }
            catch (Exception)
            {
                MessageBox.Show("Error al editar la cita");
                ExecuteEditCiteCommand(parameter);
                return;
            }

            var response = await _citeController.Update(updatedCite);

            if (response.Success)
            {
                Cites[index] = updatedCite;
                MessageBox.Show("Cita actualizada con éxito");
            }
            else
            {
                MessageBox.Show("Error al actualizar la cita");
            }
        }

        private async void ExecuteDeleteCiteCommand(object parameter)
        {
            if (SelectedCite != null)
            {
                var response = await _citeController.Delete(SelectedCite.Id.Value);
                if (response.Success)
                {
                    Cites.Remove(SelectedCite);
                }
                else
                {
                    MessageBox.Show("Error al eliminar la cita");
                }
            }

        }

        private async void ExecuteAddVisitCommandAsync(object parameter)
        {
            if (SelectedCite == null)
            {
                MessageBox.Show("Seleccione una cita para editar");
                return;
            }

            int index = Cites.IndexOf(SelectedCite);
            Cite cite = SelectedCite;


            // *** Crear una nueva visita ***

            VisitForm dialog = new();
            bool? result = dialog.ShowDialog();

            if (result == false) return;

            Visit newVisit;

            try
            {
                newVisit = Visit.FromPrimitives(
                    0,
                    type: dialog.cmbType.SelectedItem.ToString() ?? throw new ArgumentException(),
                    classification: dialog.txtClasificacion.Text,
                    description: dialog.txtDescripcion.Text,
                    isComunicated: dialog.chkIsCommunicated.IsChecked == true,
                    isDerived: dialog.chkIsDerived.IsChecked == true,
                    traumaType: dialog.chkIsDerived.IsChecked == true ? dialog.cmbTraumaType.SelectedItem.ToString() : null,
                    place: dialog.chkIsDerived.IsChecked == true ? dialog.cmbLugar.SelectedItem.ToString() : null,
                    date: dialog.dpFecha.SelectedDate ?? DateTime.Now,
                    patientId: _patient.Id.Value
                    );
            }
            catch (Exception)
            {
                MessageBox.Show("Datos de visita no validos");
                ExecuteAddVisitCommandAsync(parameter);
                return;
            }

            var response = await _visitController.Register(newVisit);

            if (!response.Success)
            {
                MessageBox.Show("Error al crear la visita");
                return;
            }

            // *** Actualizar la cita seleccionada con la nueva visita ***
            cite.VisitId = new VisitId(response.Id);

            var updateCiteResponse = await _citeController.Update(cite);

            if (updateCiteResponse.Success)
            {
                MessageBox.Show("Visita creada con éxito");
                Cites[index] = cite;
            }
            else
            {
                MessageBox.Show("Error al actualizar la cita");
            }
        }



        private async void LoadCites()
        {
            var response = await _citeController.SearchByPatientId(_patient.Id.Value);
            if (response.Success)
            {
                Cites = new ObservableCollection<Cite>(response.Data ?? new List<Cite>());
            }
            else
            {
                MessageBox.Show("Error al cargar las citas del paciente");
            }
        }
    }
}
