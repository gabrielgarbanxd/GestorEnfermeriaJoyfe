using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FontAwesome.Sharp;
using GestorEnfermeriaJoyfe.Adapters.PatientAdapters;
using GestorEnfermeriaJoyfe.Adapters.UserAdapters;
using GestorEnfermeriaJoyfe.Domain.Patient;
using GestorEnfermeriaJoyfe.Domain.User;
using GestorEnfermeriaJoyfe.Infraestructure;

namespace GestorEnfermeriaJoyfe.UI.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        // //===>> Fields <<====//
        private List<Patient> _pacientes;

        private ViewModelBase _currentPageView;
        private string _title;
        private IconChar _icon;

        private readonly PatientController PatientController = new();

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

            // Suscripcion a los eventos del mediador
            PacientesMediator.Instance.PacienteUpdated += UpdatePaciente;
            PacientesMediator.Instance.PacienteDeleted += DeletePaciente;
            PacientesMediator.Instance.PacienteCreated += AddPaciente;
        }

        public override async Task OnMountedAsync()
        {
            await LoadPacientes();
        }

        // //===>> Commands Methods <<====//
        private void ExecuteShowPrincipalViewCommand(object? obj)
        {
            CurrentPageView = new PrincipalViewModel();
            Title = "Estadisticas Principales";
            Icon = IconChar.Home;
        }
        private void ExecuteShowPacientesViewCommand(object obj)
        {
            CurrentPageView = new PacientesViewModel(_pacientes);
            Title = "Pacientes";
            Icon = IconChar.HospitalUser;
        }
        private void ExecuteShowCalendarioViewCommand(object obj)
        {
            CurrentPageView = new CalendarViewModel();
            Title = "Calendario";
            Icon = IconChar.CalendarAlt;
        }

        private async Task LoadPacientes()
        {
            var response = await PatientController.GetAll();
            
            if (response.Success)
            {
                _pacientes = response.Data;
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
