using GestorEnfermeriaJoyfe.Adapters.CalendarAdapters;
using GestorEnfermeriaJoyfe.Domain.Calendar;
using GestorEnfermeriaJoyfe.Domain.Calendar.ValueObjects;
using GestorEnfermeriaJoyfe.UI.Views;
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
        private readonly CalendarController _calendarController = new();

        private ObservableCollection<GestorEnfermeriaJoyfe.Domain.Calendar.Calendar> _calendarEntries;
        public ObservableCollection<GestorEnfermeriaJoyfe.Domain.Calendar.Calendar> CalendarEntries
        {
            get => _calendarEntries;
            set
            {
                _calendarEntries = value;
                OnPropertyChanged(nameof(CalendarEntries));
            }
        }

        private readonly ICollectionView _filteredCalendarEntriesView;
        public ICollectionView FilteredCalendarEntriesView
        {
            get { return _filteredCalendarEntriesView; }
        }

        private GestorEnfermeriaJoyfe.Domain.Calendar.Calendar _selectedCalendarEntry;
        public GestorEnfermeriaJoyfe.Domain.Calendar.Calendar SelectedCalendarEntry
        {
            get => _selectedCalendarEntry;
            set
            {
                _selectedCalendarEntry = value;
                OnPropertyChanged(nameof(SelectedCalendarEntry));
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
                    FilterCalendarEntries();
                }

            }
        }

        public ICommand CreateCalendarEntryCommand { get; }
        public ICommand EditCalendarEntryCommand { get; }
        public ICommand DeleteCalendarEntryCommand { get; }

        public CalendarViewModel()
        {
            CreateCalendarEntryCommand = new ViewModelCommand(ExecuteCreateCalendarEntryCommand);
            EditCalendarEntryCommand = new ViewModelCommand(ExecuteEditCalendarEntryCommand);
            DeleteCalendarEntryCommand = new ViewModelCommand(ExecuteDeleteCalendarEntryCommand);

            CalendarEntries = new ObservableCollection<GestorEnfermeriaJoyfe.Domain.Calendar.Calendar>();
            _filteredCalendarEntriesView = CollectionViewSource.GetDefaultView(CalendarEntries);
            _filteredCalendarEntriesView.Filter = CalendarEntryFilter;

            LoadCalendarEntries();
        }

        private async void LoadCalendarEntries()
        {
            var response = await _calendarController.GetAll();

            if (response.Success)
            {
                CalendarEntries = new ObservableCollection<GestorEnfermeriaJoyfe.Domain.Calendar.Calendar>(response.Data);
            }
            else
            {
                MessageBox.Show("Error al cargar las entradas del calendario");
            }
        }

        private async void ExecuteCreateCalendarEntryCommand(object obj)
        {

        }

        private async void ExecuteEditCalendarEntryCommand(object obj)
        {

        }

        private async void ExecuteDeleteCalendarEntryCommand(object obj)
        {
        }

        private bool CalendarEntryFilter(object item)
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                return true;
            }

            string lowerCaseSearchText = SearchText.ToLower();

            GestorEnfermeriaJoyfe.Domain.Calendar.Calendar calendarEntry = item as GestorEnfermeriaJoyfe.Domain.Calendar.Calendar;

            return
                Regex.IsMatch(calendarEntry.Task.Value, lowerCaseSearchText, RegexOptions.IgnoreCase);
        }

        public void FilterCalendarEntries()
        {
            try
            {
                _filteredCalendarEntriesView.Refresh();
            }
            catch (Exception)
            {
                SearchText = "";
                MessageBox.Show("Error al filtrar las entradas del calendario");
            }
        }
    }
}
