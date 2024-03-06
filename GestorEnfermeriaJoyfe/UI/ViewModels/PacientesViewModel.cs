using GestorEnfermeriaJoyfe.UI.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using GestorEnfermeriaJoyfe.Domain.Patient;
using GestorEnfermeriaJoyfe.Adapters.PatientAdapters;
using GestorEnfermeriaJoyfe.Domain.Patient.ValueObjects;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Windows.Data;

namespace GestorEnfermeriaJoyfe.UI.ViewModels
{
    public class PacientesViewModel : ViewModelBase
    {
        private readonly PatientController PatientController;

        // //===>> Fields <<====//
        private ObservableCollection<Patient> _pacientes;
        public ObservableCollection<Patient> Pacientes
        {
            get => _pacientes;
            set
            {
                _pacientes = value;
                OnPropertyChanged(nameof(Pacientes));
            }
        }


        private readonly ICollectionView _filteredPacientesView;
        public ICollectionView FilteredPacientesView
        {
            get { return _filteredPacientesView; }
        }

        private Patient _selectedPaciente;
        public Patient SelectedPaciente
        {
            get => _selectedPaciente;
            set
            {
                _selectedPaciente = value;
                OnPropertyChanged(nameof(SelectedPaciente));
            }
        }

        private string _searchText = "";
        public string SearchText 
        { 
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged(nameof(SearchText));
                    FilterPacientes();
                }

            }
        }

        //===>> Commands <<====//
        public ICommand CreatePacienteCommand { get; }
        public ICommand EditPacienteCommand { get; }
        public ICommand DeletePacienteCommand { get; }
        public ICommand DoubleClickPacienteCommand { get; }


        //===>> Constructor <<====//
        public PacientesViewModel(List<Patient> pacientes, PatientController patientController)
        {
            this.PatientController = patientController;
            Pacientes = new ObservableCollection<Patient>(pacientes);

            _filteredPacientesView = CollectionViewSource.GetDefaultView(Pacientes);
            _filteredPacientesView.Filter = PatientFilter;

            // *** Carga Commands ***
            CreatePacienteCommand = new ViewModelCommand(ExecuteCreatePacienteCommand);
            EditPacienteCommand = new ViewModelCommand(ExecuteEditPacienteCommand);
            DeletePacienteCommand = new ViewModelCommand(ExecuteDeletePacienteCommand);
            DoubleClickPacienteCommand = new ViewModelCommand(ExecuteDoubleClickPacienteCommand);
        }

        //===>> Commands Methods <<====//
        private async void ExecuteCreatePacienteCommand(object obj)
        {
            PacienteForm dialog = new();
            bool? result = dialog.ShowDialog();

              
            if (result == false)
            {
                return;
            }

            Patient newPatient;
            try
            {
                newPatient = Patient.FromPrimitives(0, dialog.txtNombre.Text, dialog.txtApellido1.Text, dialog.txtApellido2.Text, dialog.txtCurso.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Datos de paciente no validos");
                ExecuteCreatePacienteCommand(null);
                return;
            }


            var response = await PatientController.Register(newPatient);

            if (response.Success)
            {
                MessageBox.Show("Paciente creado con éxito");

                newPatient.SetId(new PatientId(response.Id));

                Pacientes.Add(newPatient);
                // Notificar al mediador que se ha creado un nuevo paciente
                PacientesMediator.Instance.OnPacienteCreated(newPatient);
            }
            else
            {
                MessageBox.Show("Error al crear el paciente");
            }
        }


        private async void ExecuteEditPacienteCommand(object obj)
        {
            if (SelectedPaciente == null)
            {
                MessageBox.Show("Debe seleccionar un paciente");
                return;
            }

            int idPaciente = SelectedPaciente.Id.Value;
            int index = Pacientes.IndexOf(SelectedPaciente);

            // Crear una nueva instancia de la ventana de diálogo
            PacienteForm dialog = new(SelectedPaciente);

            // Mostrar la ventana de diálogo
            bool? result = dialog.ShowDialog();

            if (result == false)
            {
                return;
            }


            Patient updatedPaciente;

            try
            {
                updatedPaciente = Patient.FromPrimitives(idPaciente, dialog.txtNombre.Text, dialog.txtApellido1.Text, dialog.txtApellido2.Text, dialog.txtCurso.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Datos de paciente no validos");
                ExecuteEditPacienteCommand(null);
                return;
            }


            var response = await PatientController.Update(updatedPaciente);

            if (response.Success)
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

        private async void ExecuteDeletePacienteCommand(object obj)
        {
            if (SelectedPaciente == null)
            {
                MessageBox.Show("Debe seleccionar un paciente");
                return;
            }

            var pacienteAEliminar = SelectedPaciente;

            var response = await PatientController.Delete(SelectedPaciente.Id.Value);

            if (response.Success)
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


        private void ExecuteDoubleClickPacienteCommand(object obj)
        {
            if (SelectedPaciente == null)
            {
                MessageBox.Show("Debe seleccionar un paciente");
                return;
            }
            MainViewModelRouter.Instance.OnShowSinglePacienteView(SelectedPaciente);
        }

        //===>> Private Methods <<====//
        private bool PatientFilter(object item)
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                return true;
            }

            string lowerCaseSearchText = SearchText.ToLower();

            Patient? paciente = item as Patient;

            return
                Regex.IsMatch(paciente.Name.Value, lowerCaseSearchText, RegexOptions.IgnoreCase) ||
                Regex.IsMatch(paciente.LastName.Value, lowerCaseSearchText, RegexOptions.IgnoreCase) ||
                Regex.IsMatch(paciente.LastName2.Value, lowerCaseSearchText, RegexOptions.IgnoreCase) ||
                Regex.IsMatch(paciente.Course.Value.Replace("º", ""), lowerCaseSearchText, RegexOptions.IgnoreCase);
        }

        public void FilterPacientes()
        {
            try
            {
                _filteredPacientesView.Refresh();
            }
            catch (Exception)
            {
                SearchText = "";
                MessageBox.Show("Error al filtrar pacientes");
            }
        }
    }
}
