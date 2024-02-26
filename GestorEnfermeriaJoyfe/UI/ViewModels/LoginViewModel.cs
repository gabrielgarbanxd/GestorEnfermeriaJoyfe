using System;
using System.Security;
using System.Windows.Input;
using System.Threading;
using System.Security.Principal;
using GestorEnfermeriaJoyfe.UI.Views;
using GestorEnfermeriaJoyfe.Adapters.UserAdapters;
using GestorEnfermeriaJoyfe.Domain.User;
using GestorEnfermeriaJoyfe.Adapters;
using System.Windows;

namespace GestorEnfermeriaJoyfe.UI.ViewModels
{
    // Clase LoginViewModel: Representa el ViewModel para la interfaz de usuario del inicio de sesión.
    public class LoginViewModel : ViewModelBase
    {
        // Campos privados
        private string _email = "admin@joyfe.com";
        private SecureString _password;
        private string _errorMessage;
        private bool _isViewVisible = true;

        // Propiedades públicas
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
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
            LoginCommand = new ViewModelCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
            RecoverPasswordCommand = new ViewModelCommand(p => ExecuteRecoverPasswordCommand("", ""));
        }

        // Método privado para determinar si el comando de inicio de sesión puede ejecutarse
        private bool CanExecuteLoginCommand(object obj)
        {
            bool validData;
            if (string.IsNullOrWhiteSpace(Email) || Email.Length < 3 ||
                Password == null || Password.Length < 3)
                validData = false;
            else
                validData = true;
            return validData;
        }

        // Método privado para ejecutar el comando de inicio de sesión
        private async void ExecuteLoginCommand(object obj)
        {
            var userController = new UserController();
            //var isValidUser = userRepository.AuthenticateUser(new System.Net.NetworkCredential(Username, Password));
            var response = await userController.Login(Email, Password);

            PatientController2 patientController2 = new();
            var response2 = await patientController2.GetAll();
            if ( response2.Success)
            {
                MessageBox.Show("Pacientes cargados 2 ");
            }
            else
            {
                MessageBox.Show("Error al cargar pacientes 2 ");
            }

            if (response.Success)
            {
                //Thread.CurrentPrincipal = new GenericPrincipal(
                //    new GenericIdentity(response.Data.ToString()), null);
                //IsViewVisible = false;

                // Abrir MainWindow
                MainView mainView = new();
                mainView.Show();
            }
            else
            {
                ErrorMessage = "* El email o la contraseña no son válidos";
            }
        }

        // Método privado para ejecutar el comando de recuperar contraseña (no implementado actualmente)
        private void ExecuteRecoverPasswordCommand(string username, string email)
        {
            throw new NotImplementedException();
        }
    }
}
