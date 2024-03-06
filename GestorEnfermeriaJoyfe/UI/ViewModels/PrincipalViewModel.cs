using GestorEnfermeriaJoyfe.Adapters.CiteAdapters;
using GestorEnfermeriaJoyfe.Adapters.VisitAdapters;
using GestorEnfermeriaJoyfe.Domain.Cite;
using GestorEnfermeriaJoyfe.Domain.Patient;
using GestorEnfermeriaJoyfe.Domain.Patient.ValueObjects;
using GestorEnfermeriaJoyfe.Domain.Visit;
using GestorEnfermeriaJoyfe.Domain.Visit.ValueObjects;
using GestorEnfermeriaJoyfe.UI.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GestorEnfermeriaJoyfe.UI.ViewModels
{
    public class PrincipalViewModel : ViewModelBase
    {
        // //===>> Fields <<====//
        
        // *** Controllers *** //
        private readonly CiteController CiteController;
        private readonly VisitController VisitController;


        // *** Cites *** //
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


        // *** Visits *** //
        private ObservableCollection<Visit> _visits;
        public ObservableCollection<Visit> Visits
        {
            get => _visits;
            set
            {
                _visits = value;
                OnPropertyChanged(nameof(Visits));
            }
        }

        private Visit _selectedVisit;
        public Visit SelectedVisit
        {
            get => _selectedVisit;
            set
            {
                _selectedVisit = value;
                OnPropertyChanged(nameof(SelectedVisit));
            }
        }

        //===>> Commands <<====//
        public ICommand AddVisitCommand { get; }
        public ICommand DoubleClickCiteCommand { get; }
        public ICommand DoubleClickVisitCommand { get; }


        // ====>> Constructor <<====//
        public PrincipalViewModel(CiteController citeController, VisitController visitController)
        {
            CiteController = citeController;
            VisitController = visitController;

            // *** Commands *** //
            AddVisitCommand = new ViewModelCommand(ExecuteAddVisitCommandAsync);
            DoubleClickCiteCommand = new ViewModelCommand(ExecuteDoubleClickCiteCommandAsync);
            DoubleClickVisitCommand = new ViewModelCommand(ExecuteDoubleClickVisitCommandAsync);

            // *** Data Load *** //
            LoadData();
        }

        //===>> Commands Methods <<====//

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
                    patientId: cite.PatientId.Value
                    );
            }
            catch (Exception)
            {
                MessageBox.Show("Datos de visita no validos");
                ExecuteAddVisitCommandAsync(parameter);
                return;
            }

            var response = await VisitController.Register(newVisit);

            if (!response.Success)
            {
                MessageBox.Show("Error al crear la visita");
                return;
            }

            // *** Actualizar la cita seleccionada con la nueva visita ***
            cite.VisitId = new VisitId(response.Id);

            var updateCiteResponse = await CiteController.Update(cite);

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

        private async void ExecuteDoubleClickCiteCommandAsync(object parameter)
        {
            if (SelectedCite == null)
            {
                MessageBox.Show("Seleccione una cita para editar");
                return;
            }

            int citeId = SelectedCite.Id.Value;
            int pacientId = SelectedCite.PatientId.Value;
            int index = Cites.IndexOf(SelectedCite);

            Citaform dialog = new(SelectedCite);
            bool? result = dialog.ShowDialog();

            if (result == false) return;

            Cite updatedCite;

            try
            {
                updatedCite = Cite.FromPrimitives(citeId, pacientId, dialog.txtNote.Text, null, dialog.dpFechaInicio.SelectedDate ?? DateTime.Now);
            }
            catch (Exception)
            {
                MessageBox.Show("Error al editar la cita");
                return;
            }

            var response = await CiteController.Update(updatedCite);

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

        private async void ExecuteDoubleClickVisitCommandAsync(object parameter)
        {
            if (SelectedVisit == null)
            {
                MessageBox.Show("Seleccione una visita");
                return;
            }

            int visitId = SelectedVisit.Id.Value;
            int patientId = SelectedVisit.PatientId.Value;
            int index = Visits.IndexOf(SelectedVisit);

            VisitForm dialog = new(SelectedVisit);
            bool? result = dialog.ShowDialog();

            if (result == false) return;

            Visit updatedVisit;

            try
            {
                updatedVisit = Visit.FromPrimitives(
                    visitId,
                    type: dialog.cmbType.SelectedItem.ToString() ?? throw new ArgumentException(),
                    classification: dialog.txtClasificacion.Text,
                    description: dialog.txtDescripcion.Text,
                    isComunicated: dialog.chkIsCommunicated.IsChecked == true,
                    isDerived: dialog.chkIsDerived.IsChecked == true,
                    traumaType: dialog.chkIsDerived.IsChecked == true ? dialog.cmbTraumaType.SelectedItem.ToString() : null,
                    place: dialog.chkIsDerived.IsChecked == true ? dialog.cmbLugar.SelectedItem.ToString() : null,
                    date: dialog.dpFecha.SelectedDate ?? DateTime.Now,
                    patientId: patientId);
            }
            catch (Exception)
            {
                MessageBox.Show("Datos de visita no validos");
                return;
            }

            var response = await VisitController.Update(updatedVisit);

            if (response.Success)
            {
                MessageBox.Show("Visita actualizada con éxito");
                Visits[index] = updatedVisit;
            }
            else
            {
                MessageBox.Show("Error al actualizar la visita");
            }
        }


        //===>> Methods <<====//

        private async void LoadData()
        {
            await LoadCites();
            await LoadVisits();
        }

        private async Task LoadCites()
        {
            var response = await CiteController.SearchByDateWithPatientInfo(DateTime.Now);

            if (response.Success)
            {
                Cites = new ObservableCollection<Cite>(response.Data ?? new List<Cite>());
            }
            else
            {
                Cites = new ObservableCollection<Cite>();
            }
        }

        private async Task LoadVisits()
        {
            var response = await VisitController.SearchByDateWithPatientInfo(DateTime.Now);

            if (response.Success)
            {
                Visits = new ObservableCollection<Visit>(response.Data ?? new List<Visit>());
            }
            else
            {
                Visits = new ObservableCollection<Visit>();
            }
        }


    }
}
