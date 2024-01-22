using GestorEnfermeriaJoyfe.Domain;
using System.Windows;

namespace GestorEnfermeriaJoyfe.UI.Views
{
    /// <summary>
    /// Lógica de interacción para PacienteForm.xaml
    /// </summary>
    public partial class PacienteForm : Window
    {
        public PacienteForm()
        {
            InitializeComponent();
        }
        public PacienteForm(PacienteModel paciente)
        {
            InitializeComponent();

            // Inicializa los campos de texto con los datos del paciente
            txtNombre.Text = paciente.Nombre;
            txtApellido1.Text = paciente.Apellido1;
            txtApellido2.Text = paciente.Apellido2;
            txtCurso.Text = paciente.Curso;
        }

        private void AceptarButton_Click(object sender, RoutedEventArgs e) => this.DialogResult = true;
    }
}
