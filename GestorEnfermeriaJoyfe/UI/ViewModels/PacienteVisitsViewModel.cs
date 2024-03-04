using GestorEnfermeriaJoyfe.Adapters.VisitAdapters;
using GestorEnfermeriaJoyfe.Domain.Patient;
using GestorEnfermeriaJoyfe.Domain.Visit;
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

        // ===>> Commands <<====//
        public ICommand GoBackCommand { get; }

        public PacienteVisitsViewModel(Patient patient)
        {
            this.patient = patient;
            visitController = new VisitController();

            // *** Carga de Datos ***
            LoadVisits();

            // *** Carga Commands *** 
            GoBackCommand = new ViewModelCommand((object parameter) => MainViewModelRouter.Instance.OnShowSinglePacienteView(patient));
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
