using GestorEnfermeriaJoyfe.Adapters.CiteAdapters;
using GestorEnfermeriaJoyfe.Domain.Cite;
using GestorEnfermeriaJoyfe.Domain.Patient;
using GestorEnfermeriaJoyfe.UI.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GestorEnfermeriaJoyfe.UI.ViewModels
{
    public class PacienteCitasViewModel : ViewModelBase
    {
        private readonly Patient _patient;
        private readonly CiteController _citeController;

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

        public ICommand GoBackCommand { get; }

        public PacienteCitasViewModel(Patient patient)
        {
            _patient = patient;
            _citeController = new CiteController();

            GoBackCommand = new ViewModelCommand((object parameter) => MainViewModelRouter.Instance.OnShowSinglePacienteView(patient));

            LoadCites();
        }

        private async void LoadCites()
        {
            var response = await _citeController.SearchByPatientId(_patient.Id.Value);
            if (response.Success)
            {
                Cites = new ObservableCollection<Cite>(response.Data ?? new List<Cite>());
            }
            else
            {
                MessageBox.Show("Error al cargar las citas del paciente");
            }
        }
    }
}
