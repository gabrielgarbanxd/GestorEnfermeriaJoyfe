using GestorEnfermeriaJoyfe.Domain;
using GestorEnfermeriaJoyfe.UI.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using GestorEnfermeriaJoyfe.Infraestructure;

namespace GestorEnfermeriaJoyfe.UI.ViewModels
{
    public class PacientesViewModel : ViewModelBase
    {

        // //===>> Fields <<====//
        private ObservableCollection<PacienteModel> _pacientes;
        private IPacienteRepository pacienteRepository;

        private PacienteModel _selectedPaciente;

        // //===>> Propertys <<====//
        public ObservableCollection<PacienteModel> Pacientes
        {
            get => _pacientes;
            set
            {
                _pacientes = value;
                OnPropertyChanged(nameof(Pacientes));
            }
        }
        public PacienteModel SelectedPaciente
        {
            get => _selectedPaciente;
            set
            {
                _selectedPaciente = value;
                OnPropertyChanged(nameof(SelectedPaciente));
            }
        }

        //===>> Commands <<====//
        public ICommand CreatePacienteCommand { get; }
        public ICommand EditPacienteCommand { get; }
        public ICommand DeletePacienteCommand { get; }


        //===>> Constructor <<====//
        public PacientesViewModel(List<PacienteModel> pacientes)
        {
            pacienteRepository = new PacienteRepository();
            Pacientes = new ObservableCollection<PacienteModel>(pacientes);

            // *** Carga Commands ***
            CreatePacienteCommand = new ViewModelCommand(ExecuteCreatePacienteCommand);
            EditPacienteCommand = new ViewModelCommand(ExecuteEditPacienteCommand);
            DeletePacienteCommand = new ViewModelCommand(ExecuteDeletePacienteCommand);
        }

        //===>> Commands Methods <<====//
        private void ExecuteCreatePacienteCommand(object obj)
        {
            PacienteForm dialog = new PacienteForm();
            bool? result = dialog.ShowDialog();

            // Si el usuario hizo clic en el botón Aceptar, crear el nuevo paciente
            if (result == true)
            {
                PacienteModel nuevoPaciente = new PacienteModel()
                {
                    Nombre = dialog.txtNombre.Text,
                    Apellido1 = dialog.txtApellido1.Text,
                    Apellido2 = dialog.txtApellido2.Text,
                    Curso = dialog.txtCurso.Text,
                };

                PacienteModel pacienteCreado = pacienteRepository.Create(nuevoPaciente);

                if (pacienteCreado != null)
                {
                    MessageBox.Show("Paciente creado con éxito");
                    Pacientes.Add(pacienteCreado);
                    // Notificar al mediador que se ha creado un nuevo paciente
                    PacientesMediator.Instance.OnPacienteCreated(pacienteCreado);
                }
                else
                {
                    MessageBox.Show("Error al crear el paciente");
                }
            }
        }


        private void ExecuteEditPacienteCommand(object obj)
        {
            if (SelectedPaciente == null)
            {
                MessageBox.Show("Debe seleccionar un paciente");
                return;
            }

            int idPaciente = SelectedPaciente.Id;
            int index = Pacientes.IndexOf(SelectedPaciente);

            // Crear una nueva instancia de la ventana de diálogo
            PacienteForm dialog = new PacienteForm(SelectedPaciente);

            // Mostrar la ventana de diálogo
            bool? result = dialog.ShowDialog();

            // Si el usuario hizo clic en el botón Aceptar, actualizar el paciente
            if (result == true)
            {
                PacienteModel updatedPaciente = new PacienteModel()
                {
                    Id = idPaciente,
                    Nombre = dialog.txtNombre.Text,
                    Apellido1 = dialog.txtApellido1.Text,
                    Apellido2 = dialog.txtApellido2.Text,
                    Curso = dialog.txtCurso.Text
                };

                bool resultado = pacienteRepository.Update(updatedPaciente);

                if (resultado)
                {
                    MessageBox.Show("Paciente actualizado con éxito");

                    if (index != -1)
                    {
                        Pacientes[index] = updatedPaciente;

                        // Notificar al mediador que se ha eliminado un paciente
                        PacientesMediator.Instance.OnPacienteUpdated(updatedPaciente);
                    }

                }
                else
                {
                    MessageBox.Show("Error al actualizar el paciente");
                }
            }
        }

        private void ExecuteDeletePacienteCommand(object obj)
        {
            if (SelectedPaciente == null)
            {
                MessageBox.Show("Debe seleccionar un paciente");
                return;
            }

            PacienteModel pacienteAEliminar = SelectedPaciente;

            bool resultado = pacienteRepository.Delete(pacienteAEliminar.Id);

            if (resultado)
            {
                MessageBox.Show("Paciente eliminado con éxito");
                Pacientes.Remove(pacienteAEliminar);
                // Notificar al mediador que se ha actualizado un paciente
                PacientesMediator.Instance.OnPacienteDeleted(pacienteAEliminar);
            }
            else
            {
                MessageBox.Show("Error al eliminar el paciente");
            }
        }


    }
}
