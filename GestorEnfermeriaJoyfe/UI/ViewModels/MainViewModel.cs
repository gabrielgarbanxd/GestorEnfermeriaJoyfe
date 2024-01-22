using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using FontAwesome.Sharp;
using GestorEnfermeriaJoyfe.Domain;
using GestorEnfermeriaJoyfe.Infraestructure;

namespace GestorEnfermeriaJoyfe.UI.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        // //===>> Fields <<====//
        private UserModel _user;
        private IUserRepository userRepository;

        private IPacienteRepository pacienteRepository;
        private List<PacienteModel> _pacientes;

        private ViewModelBase _currentPageView;
        private string _title;
        private IconChar _icon;

        // //===>> Propertys <<====//
        public UserModel User
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged(nameof(User));
            }
        }

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
            // *** Carga Usuario y Pacientes ***
            userRepository = new UserRepository();
            pacienteRepository = new PacienteRepository();
            User = new UserModel();


            loadCurrentUserData();
            loadPacientes();

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

        // //===>> Commands Methods <<====//
        private void ExecuteShowPrincipalViewCommand(object obj)
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
            CurrentPageView = new CalendarioViewModel();
            Title = "Calendario";
            Icon = IconChar.CalendarAlt;
        }

        // //===>> Methods <<====//
        private void loadCurrentUserData()
        {
            // Obtiene el nombre de usuario actual del hilo principal.
            var userName = Thread.CurrentPrincipal.Identity.Name;
            //MessageBox.Show($"Nombre de usuario ingresado: {currentUserName}");

            // Utiliza el repositorio de usuarios para obtener el modelo de usuario asociado al nombre de usuario actual.
            var user = userRepository.GetByUsername(userName);

            // Verifica si se ha encontrado un usuario válido asociado al nombre de usuario actual.
            if (user != null)
            {

                User = user;
            }
            else
            {
                // Si no se encuentra un usuario válido, muestra un cuadro de mensaje informando al usuario que la sesión no pudo iniciarse correctamente.
                MessageBox.Show("El Usuario no es válido, no se ha podido iniciar sesión");
                // Cierra la aplicación.
                Application.Current.Shutdown();
            }
        }

        private void loadPacientes()
        {
            _pacientes = pacienteRepository.All();
        }

        private void UpdatePaciente(PacienteModel updatedPaciente)
        {
            // Buscar el paciente en la lista de pacientes
            int index = _pacientes.FindIndex(p => p.Id == updatedPaciente.Id);

            // Si el paciente se encuentra en la lista, actualizarlo
            if (index != -1)
            {
                _pacientes[index] = updatedPaciente;
            }
        }

        private void DeletePaciente(PacienteModel deletedPaciente)
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

        private void AddPaciente(PacienteModel newPaciente)
        {
            // Añadir el nuevo paciente a la lista
            _pacientes.Add(newPaciente);
        }


    }
}
