﻿using GestorEnfermeriaJoyfe.Domain.Cite;
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
    /// Lógica de interacción para Citaform.xaml
    /// </summary>
    public partial class Citaform : Window
    {
        public Citaform()
        {
            InitializeComponent();
        }

        public Citaform(Cite cite)
        {
            InitializeComponent();
            txtNote.Text = cite.Note.Value;
            dpFechaInicio.SelectedDate = cite.Date.Value;
        }

        private void AceptarButton_Click(object sender, RoutedEventArgs e) => this.DialogResult = true;
    }
}
