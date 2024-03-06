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
        //private readonly CiteController _citeController = new CiteController();

        //private ObservableCollection<Cite> _cites;
        //public ObservableCollection<Cite> Cites
        //{
        //    get => _cites;
        //    set
        //    {
        //        _cites = value;
        //        OnPropertyChanged(nameof(Cites));
        //    }
        //}

        //private readonly ICollectionView _filteredCitesView;
        //public ICollectionView FilteredCitesView
        //{
        //    get { return _filteredCitesView; }
        //}

        //private Cite _selectedCite;
        //public Cite SelectedCite
        //{
        //    get => _selectedCite;
        //    set
        //    {
        //        _selectedCite = value;
        //        OnPropertyChanged(nameof(SelectedCite));
        //    }
        //}

        //private string _searchText = "";
        //public string SearchText
        //{
        //    get => _searchText;
        //    set
        //    {
        //        if (_searchText != value)
        //        {
        //            _searchText = value;
        //            OnPropertyChanged(nameof(SearchText));
        //            FilterCites();
        //        }

        //    }
        //}

        //public ICommand CreateCiteCommand { get; }
        //public ICommand EditCiteCommand { get; }
        //public ICommand DeleteCiteCommand { get; }

        //public ICommand CiteDateChangedCommand { get; }

        //public CalendarViewModel()
        //{
        //    CreateCiteCommand = new ViewModelCommand(ExecuteCreateCiteCommand);
        //    EditCiteCommand = new ViewModelCommand(ExecuteEditCiteCommand);
        //    DeleteCiteCommand = new ViewModelCommand(ExecuteDeleteCiteCommand);

        //    CiteDateChangedCommand = new ViewModelCommand(ExecuteSelectedDatesChangedCommand);

        //    Cites = new ObservableCollection<Cite>();
        //    _filteredCitesView = CollectionViewSource.GetDefaultView(Cites);
        //    _filteredCitesView.Filter = CiteFilter;

        //    LoadCites();
        //}

        //private async void LoadCites()
        //{
        //    var response = await _citeController.GetAll();

        //    if (response.Success)
        //    {
        //        Cites = new ObservableCollection<Cite>(response.Data);
        //    }
        //    else
        //    {
        //        MessageBox.Show("Error al cargar las citas");
        //    }
        //}

        //private void ExecuteSelectedDatesChangedCommand(object obj)
        //{
        //    MessageBox.Show("El comando SelectedDatesChangedCommand no está implementado.");
        //}

        //private async void ExecuteCreateCiteCommand(object obj)
        //{
        //    if (obj is string action && action == "AgregarNota")
        //    {
        //        string newNote = Microsoft.VisualBasic.Interaction.InputBox("Ingrese la nueva nota:", "Agregar Nota");

        //        if (!string.IsNullOrWhiteSpace(newNote))
        //        {
        //            DateTime currentDateTime = DateTime.Now;

        //            Cite newCite = new Cite(null, new PatientId(0), new CiteNote(newNote), null, new CiteDate(currentDateTime));

        //            var response = await _citeController.Create(newCite);

        //            if (response.Success)
        //            {
        //                MessageBox.Show("Nota agregada con éxito.");
        //            }
        //            else
        //            {
        //                MessageBox.Show("Error al agregar la nota.");
        //            }
        //        }
        //        else
        //        {
        //            MessageBox.Show("Por favor ingrese una nota válida.");
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("El parámetro pasado no es válido para agregar una nota.");
        //    }
        //}

        //private async void ExecuteEditCiteCommand(object obj)
        //{
        //    if (obj is Cite cite)
        //    {
        //        var response = await _citeController.Update(cite);

        //        if (response.Success)
        //        {
        //            MessageBox.Show("Cita actualizada con éxito.");
        //        }
        //        else
        //        {
        //            MessageBox.Show("Error al actualizar la cita.");
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("El objeto pasado como parámetro no es una instancia de Cite.");
        //    }
        //}

        //private async void ExecuteDeleteCiteCommand(object obj)
        //{
        //    if (obj is Cite cite)
        //    {
        //        var response = await _citeController.Delete(cite.Id.Value);

        //        if (response.Success)
        //        {
        //            MessageBox.Show("Cita eliminada con éxito.");
        //        }
        //        else
        //        {
        //            MessageBox.Show("Error al eliminar la cita.");
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("El objeto pasado como parámetro no es una cita.");
        //    }
        //}

        //private bool CiteFilter(object item)
        //{
        //    if (string.IsNullOrWhiteSpace(SearchText))
        //    {
        //        return true;
        //    }

        //    string lowerCaseSearchText = SearchText.ToLower();

        //    Cite cite = item as Cite;

        //    return Regex.IsMatch(cite.Note.Value, lowerCaseSearchText, RegexOptions.IgnoreCase);
        //}

        //public void FilterCites()
        //{
        //    try
        //    {
        //        _filteredCitesView.Refresh();
        //    }
        //    catch (Exception)
        //    {
        //        SearchText = "";
        //        MessageBox.Show("Error al filtrar las citas.");
        //    }
        //}
    }
}
