using GestorEnfermeriaJoyfe.Domain.Visit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GestorEnfermeriaJoyfe.UI.Views
{
    /// <summary>
    /// Lógica de interacción para VisitForm.xaml
    /// </summary>
    public partial class VisitForm : Window
    {
        public VisitForm()
        {
            InitializeComponent();
        }

        public VisitForm(Visit visit)
        {
            InitializeComponent();

            foreach (ComboBoxItem item in cmbType.Items)
            {
                if (item.Content.ToString() == visit.Type.Value)
                {
                    cmbType.SelectedItem = item;
                    break;
                }
            }


            txtClasificacion.Text = visit.Classification.Value;
            txtDescripcion.Text = visit.Description?.Value ?? "";

            if (visit.IsComunicated.Value) chkIsCommunicated.IsChecked = true;
            if (visit.IsDerived.Value) chkIsDerived.IsChecked = true;

            if (visit.IsComunicated.Value == true)
            {
                foreach (ComboBoxItem item in cmbTraumaType.Items)
                {
                    if (item.Content.ToString() == visit.TraumaType?.Value)
                    {
                        cmbTraumaType.SelectedItem = item;
                        break;
                    }
                }

                foreach (ComboBoxItem item in cmbLugar.Items)
                {
                    if (item.Content.ToString() == visit.Place?.Value)
                    {
                        cmbLugar.SelectedItem = item;
                        break;
                    }
                }

            }

            dpFecha.SelectedDate = visit.Date.Value;
        }

        private void AceptarButton_Click(object sender, RoutedEventArgs e) => this.DialogResult = true;

    }
}
