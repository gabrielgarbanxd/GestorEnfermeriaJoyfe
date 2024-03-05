using GestorEnfermeriaJoyfe.Domain.Cite;
using GestorEnfermeriaJoyfe.Domain.ScheduledCiteRule;
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

        public CitaProgramadaForm(ScheduledCiteRule scheduledCiteRule)
        {
            txtNombre.Text = scheduledCiteRule.Name.Value;
            txtHora.Text = scheduledCiteRule.Hour.GetHoures;
            txtMinutos.Text = scheduledCiteRule.Hour.GetMinutes;
            dpFechaInicio.SelectedDate = scheduledCiteRule.StartDate.Value;
            dpFechaFin.SelectedDate = scheduledCiteRule.EndDate.Value;

            if (scheduledCiteRule.Lunes.Value) lstDiasSemana.SelectedItems.Add("Lunes");
            if (scheduledCiteRule.Martes.Value) lstDiasSemana.SelectedItems.Add("Martes");
            if (scheduledCiteRule.Miercoles.Value) lstDiasSemana.SelectedItems.Add("Miercoles");
            if (scheduledCiteRule.Jueves.Value) lstDiasSemana.SelectedItems.Add("Jueves");
            if (scheduledCiteRule.Viernes.Value) lstDiasSemana.SelectedItems.Add("Viernes");


            InitializeComponent();
        }

        private void AceptarButton_Click(object sender, RoutedEventArgs e) => this.DialogResult = true;


    }
}
