using GestorEnfermeriaJoyfe.Domain.Patient;
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
        public PacienteForm(Patient paciente)
        {
            InitializeComponent();

            // Inicializa los campos de texto con los datos del paciente
            txtNombre.Text = paciente.Name.Value;
            txtApellido1.Text = paciente.LastName.Value;
            txtApellido2.Text = paciente.LastName2.Value;
            txtCurso.Text = paciente.Course.Value;
        }

        private void AceptarButton_Click(object sender, RoutedEventArgs e) => this.DialogResult = true;
    }
}
