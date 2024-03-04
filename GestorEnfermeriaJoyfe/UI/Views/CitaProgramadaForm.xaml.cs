using GestorEnfermeriaJoyfe.Domain.Cite;
using System;
using System.Security.Policy;
using System.Windows;
using System.Windows.Controls;

namespace GestorEnfermeriaJoyfe.UI.Views
{
    /// <summary>
    /// Lógica de interacción para CitaProgramadaForm.xaml
    /// </summary>
    public partial class CitaProgramadaForm : Window
    {
        public CitaProgramadaForm()
        {
            InitializeComponent();
        }

        public CitaProgramadaForm(Cite cite)
        {
            InitializeComponent();

            

        }

        private void AceptarButton_Click(object sender, RoutedEventArgs e) => this.DialogResult = true;


    }
}
