using GestorEnfermeriaJoyfe.Adapters.CiteAdapters;
using GestorEnfermeriaJoyfe.Adapters.VisitAdapters;
using GestorEnfermeriaJoyfe.Domain.Cite;
using GestorEnfermeriaJoyfe.Domain.Visit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.UI.ViewModels
{
    public class PrincipalViewModel : ViewModelBase
    {
        // //===>> Fields <<====//
        
        // *** Controllers *** //
        private readonly CiteController CiteController;
        private readonly VisitController VisitController;


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


        // *** Visits *** //
        private ObservableCollection<Visit> _visits;
        public ObservableCollection<Visit> Visits
        {
            get => _visits;
            set
            {
                _visits = value;
                OnPropertyChanged(nameof(Visits));
            }
        }

        private Visit _selectedVisit;
        public Visit SelectedVisit
        {
            get => _selectedVisit;
            set
            {
                _selectedVisit = value;
                OnPropertyChanged(nameof(SelectedVisit));
            }
        }

        //===>> Commands <<====//

        // ====>> Constructor <<====//
        public PrincipalViewModel(CiteController citeController, VisitController visitController)
        {
            CiteController = citeController;
            VisitController = visitController;

            // *** Data Load *** //
            LoadData();
        }

        //===>> Commands Methods <<====//

        //===>> Methods <<====//

        private async void LoadData()
        {
            await LoadCites();
            await LoadVisits();
        }

        private async Task LoadCites()
        {
            var response = await CiteController.SearchByDateWithPatientInfo(DateTime.Now);

            if (response.Success)
            {
                Cites = new ObservableCollection<Cite>(response.Data ?? new List<Cite>());
            }
            else
            {
                Cites = new ObservableCollection<Cite>();
            }
        }

        private async Task LoadVisits()
        {
            var response = await VisitController.SearchByDateWithPatientInfo(DateTime.Now);

            if (response.Success)
            {
                Visits = new ObservableCollection<Visit>(response.Data ?? new List<Visit>());
            }
            else
            {
                Visits = new ObservableCollection<Visit>();
            }
        }


    }
}
