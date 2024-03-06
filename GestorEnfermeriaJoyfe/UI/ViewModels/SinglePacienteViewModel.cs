   using GestorEnfermeriaJoyfe.Adapters.ScheduledCiteRuleAdapters;
using GestorEnfermeriaJoyfe.Domain.Patient;
using GestorEnfermeriaJoyfe.Domain.ScheduledCiteRule;
using GestorEnfermeriaJoyfe.Domain.ScheduledCiteRule.ValueObjects;
using GestorEnfermeriaJoyfe.UI.Views;
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

        // //===>> Fields <<====//
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

        // ====>> Constructor <<====//
        public SinglePacienteViewModel(Patient patient, ScheduledCiteRuleController scheduledCiteRuleController)
        {
            this.patient = patient;
            this.scheduledCiteRuleController = scheduledCiteRuleController;

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

        private async void ExecuteCreateScheduleCommand(object obj)
        {
            CitaProgramadaForm dialog = new();
            bool? result = dialog.ShowDialog();

            if (result == false) return;

            var diasSemana = dialog.lstDiasSemana.SelectedItems.Contains("Lunes");

            ScheduledCiteRule newScheduledCiteRule;

            try
            {
                newScheduledCiteRule = ScheduledCiteRule.FromPrimitives(
                    0,
                    dialog.txtNombre.Text,
                    new TimeSpan(int.Parse(dialog.txtHora.Text), int.Parse(dialog.txtMinutos.Text), 0),
                    startDate: dialog.dpFechaInicio.SelectedDate ?? throw new ArgumentException(),
                    dialog.dpFechaFin.SelectedDate ?? throw new ArgumentException(),
                    dialog.lstDiasSemana.SelectedItems.Contains("Lunes"),
                    dialog.lstDiasSemana.SelectedItems.Contains("Martes"),
                    dialog.lstDiasSemana.SelectedItems.Contains("Miercoles"),
                    dialog.lstDiasSemana.SelectedItems.Contains("Jueves"),
                    dialog.lstDiasSemana.SelectedItems.Contains("Viernes"),
                    this.patient.Id.Value); ;

            }
            catch (Exception)
            {
                MessageBox.Show("Error al crear la cita programada");
                ExecuteCreateScheduleCommand(obj);
                return;
            }

            var response = await scheduledCiteRuleController.Create(newScheduledCiteRule);

            if (response.Success)
            {
                MessageBox.Show("Cita programada creada con éxito");
                newScheduledCiteRule.SetId(new ScheduledCiteRuleId(response.Id));
                ScheduledCiteRules.Add(newScheduledCiteRule);
            }
            else
            {
                MessageBox.Show("Error al crear la cita programada");
            }
        }

        private async void ExecuteEditScheduleCommand(object obj)
        {
            if (SelectedScheduledCiteRule == null)
            {
                MessageBox.Show("Debe seleccionar una cita programada");
                return;
            }

            int idCitaProgramada = SelectedScheduledCiteRule.Id.Value;
            int index = ScheduledCiteRules.IndexOf(SelectedScheduledCiteRule);

            CitaProgramadaForm dialog = new(SelectedScheduledCiteRule);
            bool? result = dialog.ShowDialog();

            if (result == false) return;

            ScheduledCiteRule newScheduledCiteRule;

            try
            {
                newScheduledCiteRule = ScheduledCiteRule.FromPrimitives(
                    idCitaProgramada,
                    dialog.txtNombre.Text,
                    new TimeSpan(int.Parse(dialog.txtHora.Text), int.Parse(dialog.txtMinutos.Text), 0),
                    startDate: dialog.dpFechaInicio.SelectedDate ?? throw new ArgumentException(),
                    dialog.dpFechaFin.SelectedDate ?? throw new ArgumentException(),
                    dialog.lstDiasSemana.SelectedItems.Contains("Lunes"),
                    dialog.lstDiasSemana.SelectedItems.Contains("Martes"),
                    dialog.lstDiasSemana.SelectedItems.Contains("Miercoles"),
                                                                                                                                                                                               dialog.lstDiasSemana.SelectedItems.Contains("Jueves"),
                                                                                                                                                                                                                  dialog.lstDiasSemana.SelectedItems.Contains("Viernes"),
                                                                                                                                                                                                                                     this.patient.Id.Value); ;

            }
            catch (Exception)
            {
                MessageBox.Show("Error al editar la cita programada");
                ExecuteEditScheduleCommand(obj);
                return;
            }

            var response = await scheduledCiteRuleController.Update(newScheduledCiteRule);

            if (response.Success)
            {
                MessageBox.Show("Cita programada editada con éxito");
                ScheduledCiteRules[index] = newScheduledCiteRule;
            }
            else
            {
                MessageBox.Show("Error al editar la cita programada");
            }

        }

        private async void ExecuteDeleteScheduleCommand(object obj)
        {
            if (SelectedScheduledCiteRule == null)
            {
                MessageBox.Show("Debe seleccionar una cita programada");
                return;
            }

            var reglaToDelete = SelectedScheduledCiteRule;

            var response = await scheduledCiteRuleController.Delete(reglaToDelete.Id.Value);

            if (response.Success)
            {
                MessageBox.Show("Cita programada eliminada con éxito");
                ScheduledCiteRules.Remove(reglaToDelete);
            }
            else
            {
                MessageBox.Show("Error al eliminar la cita programada");
            }

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
