using GestorEnfermeriaJoyfe.Adapters.VisitAdapters;
using GestorEnfermeriaJoyfe.Domain.Patient;
using GestorEnfermeriaJoyfe.Domain.Visit;
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

        public PacienteVisitsViewModel(Patient patient)
        {
            this.patient = patient;
            visitController = new VisitController();

            // *** Carga de Datos ***
            LoadVisits();

            // *** Carga Commands *** 
            GoBackCommand = new ViewModelCommand((object parameter) => MainViewModelRouter.Instance.OnShowSinglePacienteView(patient));
            DoubleClickVisitCommand = new ViewModelCommand(ExecuteDoubleClickVisitCommand);
            CreateVisitCommand = new ViewModelCommand(ExecuteCreateVisitCommand);
            DeleteVisitCommand = new ViewModelCommand(ExecuteDeleteVisitCommand);
        }

        // ====>> Command Methods <<====//
        private void ExecuteDoubleClickVisitCommand(object parameter)
        {
            if (SelectedVisit != null)
            {
                //MainViewModelRouter.Instance.OnShowSingleVisitView(SelectedVisit);
            }
        }

        private void ExecuteCreateVisitCommand(object parameter)
        {
            VisitForm dialog = new();
            bool? result = dialog.ShowDialog();

            if (result == false) return;

            Visit newVisit;

            try
            {
                newVisit = Visit.FromPrimitives(
                    0,
                    dialog.cmbType.SelectedItem,
                    dialog.txtClasificacion.Text,
                    dialog.txtDescripcion.Text,
                    dialog.chkIsCommunicated.IsChecked,
                    dialog.chkIsDerived.IsChecked,
                    dialog.chkIsDerived.IsChecked == true ? dialog.cmbTraumaType.SelectedItem : null,
                    dialog.chkIsDerived.IsChecked == true ? dialog.cmbLugar.SelectedItem : null,
                    DateTime.Now,
                    patient.Id.Value
                    );
            }
            catch (Exception)
            {
                MessageBox.Show("Datos de visita no validos");
                ExecuteCreateVisitCommand(null);
                return;
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
