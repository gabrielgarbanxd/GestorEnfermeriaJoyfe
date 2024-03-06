using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FontAwesome.Sharp;
using GestorEnfermeriaJoyfe.Adapters.CiteAdapters;
using GestorEnfermeriaJoyfe.Adapters.PatientAdapters;
using GestorEnfermeriaJoyfe.Adapters.ScheduledCiteRuleAdapters;
using GestorEnfermeriaJoyfe.Adapters.VisitAdapters;
using GestorEnfermeriaJoyfe.Domain.Patient;

namespace GestorEnfermeriaJoyfe.UI.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        // //===>> Controllers To Inject <<====//
        private readonly PatientController PatientController = new();
        private readonly VisitController VisitController = new();
        private readonly CiteController CiteController = new();
        private readonly ScheduledCiteRuleController ScheduledCiteRuleController = new();

        // //===>> Fields <<====//
        private List<Patient> _pacientes;
        
        private ViewModelBase _currentPageView;
        private string _title;
        private IconChar _icon;
        private int _selectedRadioButtonIndex = 0; // Índice del RadioButton "Pacientes"


        // //===>> Propertys <<====//
        public ViewModelBase CurrentPageView 
        {   
            get => _currentPageView;
            set
            {
                _currentPageView = value;
                OnPropertyChanged(nameof(CurrentPageView));
            }
        }

        public string Title 
        { 
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public IconChar Icon 
        { 
            get => _icon;
            set
            {
                _icon = value;
                OnPropertyChanged(nameof(Icon));
            }
        }

        public int SelectedRadioButtonIndex
        {
            get { return _selectedRadioButtonIndex; }
            set
            {
                if (_selectedRadioButtonIndex != value)
                {
                    _selectedRadioButtonIndex = value;
                    OnPropertyChanged(nameof(SelectedRadioButtonIndex));
                }
            }
        }


        // //===>> Commands <<====//
        public ICommand ShowPrincipalViewCommand { get; }
        public ICommand ShowPacientesViewCommand { get; }
        public ICommand ShowCalendarioViewCommand { get; }



        // //===>> Constructor <<====//
        public MainViewModel()
        {
            // *** Carga Commands ***
            ShowPrincipalViewCommand = new ViewModelCommand(ExecuteShowPrincipalViewCommand);
            ShowPacientesViewCommand = new ViewModelCommand(ExecuteShowPacientesViewCommand);
            ShowCalendarioViewCommand = new ViewModelCommand(ExecuteShowCalendarioViewCommand);

            ExecuteShowPrincipalViewCommand(null);

            // Suscripcion a los eventos del paciente mediador
            PacientesMediator.Instance.PacienteUpdated += UpdatePaciente;
            PacientesMediator.Instance.PacienteDeleted += DeletePaciente;
            PacientesMediator.Instance.PacienteCreated += AddPaciente;

            // Suscripcion a los eventos del router
            MainViewModelRouter.Instance.ShowPrincipalView += ExecuteShowPrincipalViewCommand;
            MainViewModelRouter.Instance.ShowPacientesView += ExecuteShowPacientesViewCommand;
            MainViewModelRouter.Instance.ShowSinglePacienteView += ExecuteShowSinglePacienteView;
            MainViewModelRouter.Instance.ShowCalendarView += ExecuteShowCalendarioViewCommand;
            MainViewModelRouter.Instance.ShowPacienteVisitsView += ShowPacienteVisitsView;
            MainViewModelRouter.Instance.ShowPacienteCitasView += ShowPacienteCitasView;
        }

        public override async Task OnMountedAsync()
        {
            await LoadPacientes();
        }

        // //===>> Commands Methods <<====//
        private void ExecuteShowPrincipalViewCommand(object? obj)
        {
            SelectedRadioButtonIndex = 0;
            CurrentPageView = new PrincipalViewModel(CiteController, VisitController);
            Title = "Estadisticas Principales";
            Icon = IconChar.Home;
        }
        private void ExecuteShowPacientesViewCommand(object obj)
        {
            SelectedRadioButtonIndex = 1;
            CurrentPageView = new PacientesViewModel(_pacientes, this.PatientController);
            Title = "Pacientes";
            Icon = IconChar.HospitalUser;
        }
        private void ExecuteShowCalendarioViewCommand(object obj)
        {
            SelectedRadioButtonIndex = 2;
            CurrentPageView = new CalendarViewModel();
            Title = "Calendario";
            Icon = IconChar.CalendarAlt;
        }

        // ====>> Router Extra Methods <<====//
        private void ExecuteShowSinglePacienteView(Patient paciente)
        {
            CurrentPageView = new SinglePacienteViewModel(paciente, this.ScheduledCiteRuleController);
            Title = paciente.Name.Value + " " + paciente.LastName.Value + " " + paciente.LastName2.Value + " - " + paciente.Course.Value;
            Icon = IconChar.UserAlt;
        }

        private void ShowPacienteVisitsView(Patient paciente)
        {
            CurrentPageView = new PacienteVisitsViewModel(paciente, this.VisitController);
            Title = "Visitas de " + paciente.Name.Value + " " + paciente.LastName.Value + " " + paciente.LastName2.Value + " - " + paciente.Course.Value;
            Icon = IconChar.Stethoscope;
        }

        private void ShowPacienteCitasView(Patient paciente)
        {
            CurrentPageView = new PacienteCitasViewModel(paciente, CiteController, VisitController);
            Title = "Citas de " + paciente.Name.Value + " " + paciente.LastName.Value + " " + paciente.LastName2.Value + " - " + paciente.Course.Value;
            Icon = IconChar.CalendarAlt;
        }


        // //===>> Private Methods <<====//
        private async Task LoadPacientes()
        {
            var response = await PatientController.GetAll();
            
            if (response.Success)
            {
                //_pacientes = new List<Patient>(response.Data);
                _pacientes = new List<Patient>(response.Data);
            }
            else
            {
                MessageBox.Show("No se han podido cargar los pacientes");
            }
        }

        private void UpdatePaciente(Patient updatedPaciente)
        {
            // Buscar el paciente en la lista de pacientes
            int index = _pacientes.FindIndex(p => p.Id == updatedPaciente.Id);

            // Si el paciente se encuentra en la lista, actualizarlo
            if (index != -1)
            {
                _pacientes[index] = updatedPaciente;
            }
        }

        private void DeletePaciente(Patient deletedPaciente)
        {
            // Buscar el paciente en la lista de pacientes
            int index = _pacientes.FindIndex(p => p.Id == deletedPaciente.Id);

            // Si el paciente se encuentra en la lista, eliminarlo
            if (index != -1)
            {
                _pacientes.RemoveAt(index);
            }
            _pacientes.Remove(deletedPaciente);
        }

        private void AddPaciente(Patient newPaciente)
        {
            // Añadir el nuevo paciente a la lista
            _pacientes.Add(newPaciente);
        }
    }
}
