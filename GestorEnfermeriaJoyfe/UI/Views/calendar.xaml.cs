using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace GestorEnfermeriaJoyfe.UI.Views
{
    public partial class Calendar : UserControl
    {
        private int selectedYear = 2024;
        public Calendar()
        {
            InitializeComponent();
            // Suscribirse al evento MouseLeftButtonDown
            this.MouseLeftButtonDown += Calendar_MouseLeftButtonDown;

            // Suscribirse al evento SelectedDatesChanged del control Calendar
            MyCalendar.SelectedDatesChanged += MyCalendar_SelectedDatesChanged;

            // Establecer una fecha seleccionada inicialmente para la vista previa en el diseñador
            MyCalendar.SelectedDate = DateTime.Now;

            // Suscribirse al evento Click de los botones de año
            btn2022.Click += YearButton_Click;
            btn2023.Click += YearButton_Click;
            btn2024.Click += YearButton_Click;
            btn2025.Click += YearButton_Click;
            btn2026.Click += YearButton_Click;



        }

        private void Calendar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);

            if (parentWindow != null)
            {
                parentWindow.DragMove();
            }
        }

        private void lblNote_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtNote.Focus();
        }

        private void txtNote_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtNote.Text) && txtNote.Text.Length > 0)
            {
                lblNote.Visibility = Visibility.Collapsed;
            }
            else
            {
                lblNote.Visibility = Visibility.Visible;
            }
        }

        private void lblTime_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtTime.Focus();
        }

        private void txtTime_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtTime.Text) && txtTime.Text.Length > 0)
            {
                lblTime.Visibility = Visibility.Collapsed;
            }
            else
            {
                lblTime.Visibility = Visibility.Visible;
            }
        }



        private void ChangeCalendarMonth(int month)
        {
            int year = 2024; 

            DateTime newDate = new DateTime(year, month, 1);

            MyCalendar.DisplayDate = newDate;

            txtMonth.Text = newDate.ToString("MMMM");
            txtMonth1.Text = newDate.ToString("MMMM");
            txtMonth2.Text = newDate.ToString("MMMM");
        }


        private void YearButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null && int.TryParse(button.Content.ToString(), out selectedYear))
            {
                MyCalendar.DisplayDate = new DateTime(selectedYear, MyCalendar.DisplayDate.Month, 1);
            }
        }



        private void MonthButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null && int.TryParse(button.Content.ToString(), out int month))
            {
                ChangeCalendarMonth(month);
            }
        }


        private void MyCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime selectedDate = MyCalendar.SelectedDate.GetValueOrDefault();

            txtSelectedDay.Text = selectedDate.ToString("dd");
        }


    }
}
