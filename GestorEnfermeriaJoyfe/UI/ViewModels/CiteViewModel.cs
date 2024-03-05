using GestorEnfermeriaJoyfe.UI.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using GestorEnfermeriaJoyfe.Domain.Cite;
using GestorEnfermeriaJoyfe.Adapters.CiteAdapters;
using GestorEnfermeriaJoyfe.Domain.Cite.ValueObjects;
using System.Linq;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Windows.Data;
using System.Threading.Tasks;
using GestorEnfermeriaJoyfe.Adapters.PatientAdapters;
using GestorEnfermeriaJoyfe.Domain.Patient.ValueObjects;
using GestorEnfermeriaJoyfe.Domain.Patient;

namespace GestorEnfermeriaJoyfe.UI.ViewModels
{
    public class CiteViewModel : ViewModelBase
    {
        private readonly ICiteContract _citeRepository;

        // Properties
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

        private readonly ICollectionView _filteredCitesView;
        public ICollectionView FilteredCitesView
        {
            get { return _filteredCitesView; }
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
                    FilterCites();
                }
            }
        }

        // Commands
        public ICommand CreateCiteCommand { get; }
        public ICommand EditCiteCommand { get; }
        public ICommand DeleteCiteCommand { get; }
        public ICommand DoubleClickCiteCommand { get; }

        // Constructor
        public CiteViewModel(ICiteContract citeRepository)
        {
            _citeRepository = citeRepository;

            // Load Commands
            //CreateCiteCommand = new ViewModelCommand(async (obj) => await ExecuteCreateCiteCommand());
            EditCiteCommand = new ViewModelCommand(async (obj) => await ExecuteEditCiteCommand());
            DeleteCiteCommand = new ViewModelCommand(async (obj) => await ExecuteDeleteCiteCommand());
            DoubleClickCiteCommand = new ViewModelCommand(async (obj) => await ExecuteDoubleClickCiteCommand());

            // Load Cites
            LoadCites();
            _filteredCitesView = CollectionViewSource.GetDefaultView(Cites);
            //_filteredCitesView.Filter = CiteFilter;
        }

        // Command Methods
        private async void ExecuteCreatePacienteCommand(object obj)
        {

        }

        private async Task ExecuteEditCiteCommand()
        {
            int updatedCiteId = await _citeRepository.UpdateAsync(SelectedCite);
        }

        private async Task ExecuteDeleteCiteCommand()
        {
            int deletedCiteId = await _citeRepository.DeleteAsync(SelectedCite.Id);
        }

        private async Task ExecuteDoubleClickCiteCommand()
        {
            Cite selectedCite = await _citeRepository.FindAsync(SelectedCite.Id);
        }

        // Other methods
        private async Task LoadCites()
        {
            IEnumerable<Cite> allCites = await _citeRepository.GetAllAsync();
            Cites = new ObservableCollection<Cite>(allCites);
        }

        //private bool CiteFilter(object item)
        //{
        //if (string.IsNullOrWhiteSpace(SearchText))
        //{
        //    return true;
        //}

        //string lowerCaseSearchText = SearchText.ToLower();

        //Cite cite = item as Cite;

        //return;
        //Regex.IsMatch(cite.PropertyToSearch.ToLower(), lowerCaseSearchText, RegexOptions.IgnoreCase);
        //}

        public void FilterCites()
        {
            try
            {
                _filteredCitesView.Refresh();
            }
            catch (Exception)
            {
                SearchText = "";
                MessageBox.Show("Error al filtrar citas");
            }
        }
    }
}
