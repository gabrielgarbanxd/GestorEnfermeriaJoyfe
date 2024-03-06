using GestorEnfermeriaJoyfe.Adapters.CiteAdapters;
using GestorEnfermeriaJoyfe.Domain.Cite;
using GestorEnfermeriaJoyfe.Domain.Cite.ValueObjects;
using GestorEnfermeriaJoyfe.Domain.Patient.ValueObjects;
using GestorEnfermeriaJoyfe.Domain.Shared;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
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

        // *** Commands *** //
        public ICommand DoubleClickCiteCommand { get; set; }
        public ICommand CalendarDateChangedCommand { get; set; }

        // //===>> Constructor <<====//
        public CalendarViewModel(CiteController citeController)
        {
            this.citeController = citeController;

            DoubleClickCiteCommand = new ViewModelCommand(ExecuteDoubleClickCiteCommand);
            CalendarDateChangedCommand = new ViewModelCommand(ExecuteCalendarDateChangedCommand);
        }

        // //===>> Command Methods <<====//
        private void ExecuteDoubleClickCiteCommand(object obj)
        {
            if (SelectedCite != null)
            {
                MessageBox.Show("Cita seleccionada: " + SelectedCite.Id);
            }
        }

        private async void ExecuteCalendarDateChangedCommand(object obj)
        {
            if (obj is DateTime date)
            {
                var response = await citeController.SearchByDateWithPatientInfo(date);

                if (response.Success)
                {
                    Cites = new ObservableCollection<Cite>(response.Data ?? new ObservableCollection<Cite>());
                }
            }
        }
    }
}
