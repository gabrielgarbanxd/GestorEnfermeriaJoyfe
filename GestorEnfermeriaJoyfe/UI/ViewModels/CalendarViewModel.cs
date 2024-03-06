using GestorEnfermeriaJoyfe.Adapters.CiteAdapters;
using GestorEnfermeriaJoyfe.Domain.Cite;
using GestorEnfermeriaJoyfe.Domain.Cite.ValueObjects;
using GestorEnfermeriaJoyfe.Domain.Patient.ValueObjects;
using GestorEnfermeriaJoyfe.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace GestorEnfermeriaJoyfe.UI.ViewModels
{
    public class CalendarViewModel : ViewModelBase
    {
        // //===>> Fields <<====//

        // *** Controllers *** //
        private readonly CiteController citeController;

        // *** Cites *** //
        private ObservableCollection<Cite> _cites;
        public ObservableCollection<Cite> Cites
        {
            get => _cites;
            set
            {
                _cites = value;
                OnPropertyChanged(nameof(Cites));
            }
        }

        private Cite _selectedCite;
        public Cite SelectedCite
        {
            get => _selectedCite;
            set
            {
                _selectedCite = value;
                OnPropertyChanged(nameof(SelectedCite));
            }
        }

        // *** Calendar *** //
        private DateTime _selectedDate = DateTime.Now;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
                _ = LoadCites(SelectedDate);
            }
        }

        // *** Commands *** //
        public ICommand DoubleClickCiteCommand { get; set; }

        // //===>> Constructor <<====//
        public CalendarViewModel(CiteController citeController)
        {
            this.citeController = citeController;

            // *** Commands *** //
            DoubleClickCiteCommand = new ViewModelCommand(ExecuteDoubleClickCiteCommand);

            // *** Load Data *** //
            LoadData();
        }

        // //===>> Command Methods <<====//
        private void ExecuteDoubleClickCiteCommand(object obj)
        {
            if (SelectedCite != null)
            {
                MessageBox.Show("Cita seleccionada: " + SelectedCite.Id);
            }
            MessageBox.Show("No hay ninguna cita seleccionada");
        }


        //===>> Methods <<====//

        private async void LoadData()
        {
            await LoadCites(null);
        }

        private async Task LoadCites(DateTime? dateTime)
        {
            var response = await citeController.SearchByDateWithPatientInfo(dateTime ?? DateTime.Now);

            if (response.Success)
            {

                Cites = new ObservableCollection<Cite>(response.Data ?? new List<Cite>());
            }
            else
            {
                MessageBox.Show(response.Message);
            }
        }
    }
}
