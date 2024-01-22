using System;
using System.Security;
using System.Windows.Input;
using System.Threading;
using System.Security.Principal;
using System.Windows;
using GestorEnfermeriaJoyfe.UI.Views;
using GestorEnfermeriaJoyfe.Domain;
using GestorEnfermeriaJoyfe.Infraestructure;

namespace GestorEnfermeriaJoyfe.UI.ViewModels
{
    // Clase LoginViewModel: Representa el ViewModel para la interfaz de usuario del inicio de sesión.
    public class LoginViewModel : ViewModelBase
    {
        // Campos privados
        private string _username;
        private SecureString _password;
        private string _errorMessage;
        private bool _isViewVisible = true;

        private IUserRepository userRepository;

        // Propiedades públicas
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public SecureString Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public bool IsViewVisible
        {
            get => _isViewVisible;
            set
            {
                _isViewVisible = value;
                OnPropertyChanged(nameof(IsViewVisible));
            }
        }

        // Comandos públicos
        public ICommand LoginCommand { get; }
        public ICommand RecoverPasswordCommand { get; }
        public ICommand ShowPasswordCommand { get; }
        public ICommand RemenberPasswordLoginCommand { get; }

        // Constructor
        public LoginViewModel()
        {
            userRepository = new UserRepository();
            LoginCommand = new ViewModelCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
            RecoverPasswordCommand = new ViewModelCommand(p => ExecuteRecoverPasswordCommand("", ""));
        }

        // Método privado para determinar si el comando de inicio de sesión puede ejecutarse
        private bool CanExecuteLoginCommand(object obj)
        {
            bool validData;
            if (string.IsNullOrWhiteSpace(Username) || Username.Length < 3 ||
                Password == null || Password.Length < 3)
                validData = false;
            else
                validData = true;
            return validData;
        }

        // Método privado para ejecutar el comando de inicio de sesión
        private void ExecuteLoginCommand(object obj)
        {
            var isValidUser = userRepository.AuthenticateUser(new System.Net.NetworkCredential(Username, Password));
            if (isValidUser)
            {
                UserModel user = userRepository.GetByUsername(Username); // Obtener el UserModel del usuario autenticado

                if (user != null)
                {
                    Thread.CurrentPrincipal = new GenericPrincipal(
                        new GenericIdentity(Username), null);
                    IsViewVisible = false;

                    string welcomeMessage = $"Bienvenido, {user.Name} {user.LastName}! El usuario ha sido autenticado con éxito.";
                    MessageBox.Show(welcomeMessage);

                    // Abrir MainWindow
                    MainView mainView = new MainView();
                    mainView.Show();
                }
                else
                {
                    ErrorMessage = "* El nombre de usuario o la contraseña no son válidos";
                }
            }
            else
            {
                ErrorMessage = "* El nombre de usuario o la contraseña no son válidos";
            }
        }

        // Método privado para ejecutar el comando de recuperar contraseña (no implementado actualmente)
        private void ExecuteRecoverPasswordCommand(string username, string email)
        {
            throw new NotImplementedException();
        }
    }
}
