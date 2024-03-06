using GestorEnfermeriaJoyfe.Adapters.VisitAdapters;
using GestorEnfermeriaJoyfe.Domain.Patient;
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
    public class PacienteVisitsViewModel : ViewModelBase
    {
        private readonly VisitController visitController;

        private readonly Patient patient;

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

        // ===>> Commands <<====//
        public ICommand GoBackCommand { get; }
        public ICommand DoubleClickVisitCommand { get; }
        public ICommand CreateVisitCommand { get; }
        public ICommand DeleteVisitCommand { get; }

        public PacienteVisitsViewModel(Patient patient, VisitController visitController)
        {
            this.patient = patient;
            this.visitController = visitController;

            // *** Carga de Datos ***
            LoadVisits();

            // *** Carga Commands *** 
            GoBackCommand = new ViewModelCommand((object parameter) => MainViewModelRouter.Instance.OnShowSinglePacienteView(patient));
            DoubleClickVisitCommand = new ViewModelCommand(ExecuteDoubleClickVisitCommand);
            CreateVisitCommand = new ViewModelCommand(ExecuteCreateVisitCommand);
            DeleteVisitCommand = new ViewModelCommand(ExecuteDeleteVisitCommand);
        }

        // ====>> Command Methods <<====//
        private async void ExecuteDoubleClickVisitCommand(object parameter)
        {
            if (SelectedVisit == null)
            {
                MessageBox.Show("Seleccione una visita");
                return;
            }

            int visitId = SelectedVisit.Id.Value;
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
                    patientId: patient.Id.Value);
            }
            catch (Exception)
            {
                MessageBox.Show("Datos de visita no validos");
                ExecuteDoubleClickVisitCommand(parameter);
                return;
            }

            var response = await visitController.Update(updatedVisit);

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

        private async void ExecuteCreateVisitCommand(object? parameter)
        {
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
                    patientId: patient.Id.Value
                    );
            }
            catch (Exception)
            {
                MessageBox.Show("Datos de visita no validos");
                ExecuteCreateVisitCommand(null);
                return;
            }

            var response = await visitController.Register(newVisit);

            if (response.Success)
            {
                MessageBox.Show("Visita creada con éxito");
                newVisit.SetId(new VisitId(response.Id));
                Visits.Add(newVisit);
            }
            else
            {
                MessageBox.Show("Error al crear la visita");
            }
        }

        private async void ExecuteDeleteVisitCommand(object parameter)
        {
            if (SelectedVisit != null)
            {
                var response = await visitController.Delete(SelectedVisit.Id.Value);
                if (response.Success)
                {
                    Visits.Remove(SelectedVisit);
                    MessageBox.Show("Visita eliminada con éxito");
                }
                else
                {
                    MessageBox.Show("Error al eliminar la visita");
                }
            }
        }

        // ====>> Private Methods <<====//

        private async void LoadVisits()
        {
            var response = await visitController.SearchByPatientId(patient.Id.Value);
            if (response.Success)
            {
                Visits = new ObservableCollection<Visit>(response.Data ?? new List<Visit>());
            }
            else
            {
                Visits = new();
                MessageBox.Show("Error al cargar las visitas del paciente");
            }
        }
    }
}
