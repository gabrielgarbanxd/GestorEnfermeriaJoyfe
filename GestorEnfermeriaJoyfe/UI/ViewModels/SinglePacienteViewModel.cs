using GestorEnfermeriaJoyfe.Adapters.ScheduledCiteRuleAdapters;
using GestorEnfermeriaJoyfe.Domain.Patient;
using GestorEnfermeriaJoyfe.Domain.ScheduledCiteRule;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace GestorEnfermeriaJoyfe.UI.ViewModels
{
    public class SinglePacienteViewModel : ViewModelBase
    {
        private readonly ScheduledCiteRuleController scheduledCiteRuleController;

        private readonly Patient patient;

        private ObservableCollection<ScheduledCiteRule> _scheduledCiteRules;
        public ObservableCollection<ScheduledCiteRule> ScheduledCiteRules
        {
            get => _scheduledCiteRules;
            set
            {
                _scheduledCiteRules = value;
                OnPropertyChanged(nameof(ScheduledCiteRules));
            }
        }

        private ScheduledCiteRule _selectedScheduledCiteRule;
        public ScheduledCiteRule SelectedScheduledCiteRule
        {
            get => _selectedScheduledCiteRule;
            set
            {
                _selectedScheduledCiteRule = value;
                OnPropertyChanged(nameof(SelectedScheduledCiteRule));
            }
        }

        //===>> Commands <<====//
        public ICommand GoBackCommand { get; }
        public ICommand ShowVisitsCommand { get; }
        public ICommand ShowCitesCommand { get; }
        public ICommand ShowCalendarCommand { get; }
        public ICommand CreateScheduleCommand { get; }
        public ICommand EditScheduleCommand { get; }
        public ICommand DeleteScheduleCommand { get; }

        public SinglePacienteViewModel(Patient patient)
        {
            this.patient = patient;
            scheduledCiteRuleController = new ScheduledCiteRuleController();

            // *** Carga de Datos ***
            LoadScheduledCiteRules();

            // *** Carga Commands *** 
            GoBackCommand = new ViewModelCommand(ExecuteGoBackCommand);
            ShowVisitsCommand = new ViewModelCommand(ExecuteShowVisitsCommand);
            ShowCitesCommand = new ViewModelCommand(ExecuteShowCitesCommand);
            ShowCalendarCommand = new ViewModelCommand(ExecuteShowCalendarCommand);
            CreateScheduleCommand = new ViewModelCommand(ExecuteCreateScheduleCommand);
            EditScheduleCommand = new ViewModelCommand(ExecuteEditScheduleCommand);
            DeleteScheduleCommand = new ViewModelCommand(ExecuteDeleteScheduleCommand);

        }

        //===>> Commands Methods <<====//
        private void ExecuteGoBackCommand(object obj) => MainViewModelRouter.Instance.OnShowPacientesView("Pacientes");

        private void ExecuteShowVisitsCommand(object obj) => MainViewModelRouter.Instance.OnShowPacienteVisitsView(patient);

        private void ExecuteShowCitesCommand(object obj) => MainViewModelRouter.Instance.OnShowCitasView(patient);

        private void ExecuteShowCalendarCommand(object obj) => MainViewModelRouter.Instance.OnShowCalendarView(patient);

        private void ExecuteCreateScheduleCommand(object obj)
        {
            // TODO: Implementar
        }

        private void ExecuteEditScheduleCommand(object obj)
        {

        }

        private void ExecuteDeleteScheduleCommand(object obj)
        {

        }

        // ====>> Private Methods <<====//

        private async void LoadScheduledCiteRules()
        {
            //MessageBox.Show("Cargando citas programadas de " + patient.Name.Value + " " + patient.LastName.Value + " " + patient.LastName2.Value + " - " + patient.Course.Value);
            var response = await scheduledCiteRuleController.SearchByPatientId(patient.Id.Value);

            if (response.Success)
            {
                ScheduledCiteRules = new ObservableCollection<ScheduledCiteRule>(response.Data ?? new List<ScheduledCiteRule>());
            }
            else
            {
                ScheduledCiteRules = new ObservableCollection<ScheduledCiteRule>();
                MessageBox.Show("Error al cargar las citas programadas");
            }
        }
    }
}
