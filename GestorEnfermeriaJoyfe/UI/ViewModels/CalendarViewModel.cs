using GestorEnfermeriaJoyfe.Adapters.CalendarAdapters;
using GestorEnfermeriaJoyfe.Domain.Calendar;
using GestorEnfermeriaJoyfe.Domain.Calendar.ValueObjects;
using GestorEnfermeriaJoyfe.UI.Views;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
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

        public ICommand CalendarDateChangedCommand { get; }

        public CalendarViewModel()
        {
            CreateCalendarEntryCommand = new ViewModelCommand(ExecuteCreateCalendarEntryCommand);
            EditCalendarEntryCommand = new ViewModelCommand(ExecuteEditCalendarEntryCommand);
            DeleteCalendarEntryCommand = new ViewModelCommand(ExecuteDeleteCalendarEntryCommand);

            CalendarDateChangedCommand = new ViewModelCommand(ExecuteSelectedDatesChangedCommand);

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

        private void ExecuteSelectedDatesChangedCommand(object obj)
        {
            //if (obj is SelectionChangedEventArgs e)
            //{
            //    if (e.AddedItems.Count > 0)
            //    {
            //        if (e.AddedItems[0] is DateTime selectedDate)
            //        {
            //            // Aquí puedes implementar la lógica para mostrar las entradas del calendario para la fecha seleccionada
            //            // Por ejemplo, podrías filtrar las entradas del calendario para mostrar solo las que corresponden a la fecha seleccionada
            //            // También podrías mostrar las entradas del calendario en un control de usuario personalizado o en un cuadro de diálogo
            //            // También podrías implementar la lógica para permitir al usuario agregar, editar o eliminar entradas del calendario para la fecha seleccionada
            //        }
            //    }
            //}

            MessageBox.Show("El comando SelectedDatesChangedCommand no está implementado.");
        }

        private async void ExecuteCreateCalendarEntryCommand(object obj)
        {
            // Verificar si el objeto pasado como parámetro es una cadena que indica la acción
            if (obj is string action && action == "AgregarNota")
            {
                // Mostrar un cuadro de diálogo para que el usuario ingrese la nota
                string newNote = Microsoft.VisualBasic.Interaction.InputBox("Ingrese la nueva nota:", "Agregar Nota");

                // Verificar si el usuario ingresó una nota
                if (!string.IsNullOrWhiteSpace(newNote))
                {
                    // Obtener la fecha y hora actual
                    DateTime currentDateTime = DateTime.Now;

                    // Crear la nueva entrada del calendario con la nota y la hora actual
                    GestorEnfermeriaJoyfe.Domain.Calendar.Calendar newEntry = new GestorEnfermeriaJoyfe.Domain.Calendar.Calendar(null, new CalendarFecha(currentDateTime), new CalendarTarea(newNote));

                    // Llamar al método correspondiente del controlador para crear la nueva entrada del calendario
                    var response = await _calendarController.Register(newEntry);

                    if (response.Success)
                    {
                        MessageBox.Show("Nota agregada al calendario con éxito.");
                    }
                    else
                    {
                        MessageBox.Show("Error al agregar la nota al calendario.");
                    }
                }
                else
                {
                    MessageBox.Show("Por favor ingrese una nota válida.");
                }
            }
            else
            {
                MessageBox.Show("El parámetro pasado no es válido para agregar una nota al calendario.");
            }
        }



        private async void ExecuteEditCalendarEntryCommand(object obj)
        {
            // Verificar si el objeto pasado como parámetro es una instancia de CalendarEntry
            if (obj is GestorEnfermeriaJoyfe.Domain.Calendar.Calendar calendarEntry)
            {
                // Aquí puedes implementar la lógica para editar la entrada en el calendario
                // Por ejemplo, podrías abrir un formulario para que el usuario edite los detalles de la entrada
                // Una vez que obtengas los detalles editados, puedes llamar al método correspondiente del controlador para actualizar la entrada

                // Llamar al método correspondiente del controlador para editar la entrada en el calendario
                var response = await _calendarController.Update(calendarEntry);

                if (response.Success)
                {
                    MessageBox.Show("Entrada del calendario actualizada con éxito.");
                }
                else
                {
                    MessageBox.Show("Error al actualizar la entrada del calendario.");
                }
            }
            else
            {
                MessageBox.Show("El objeto pasado como parámetro no es una instancia de CalendarEntry.");
            }
        }


        private async void ExecuteDeleteCalendarEntryCommand(object obj)
        {
            if (obj is GestorEnfermeriaJoyfe.Domain.Calendar.Calendar calendarEntry)
            {
                var response = await _calendarController.Delete(calendarEntry.Id.Value);

                if (response.Success)
                {
                    MessageBox.Show("Entrada del calendario eliminada con éxito.");
                    // Aquí podrías recargar la lista de entradas del calendario si es necesario
                }
                else
                {
                    MessageBox.Show("Error al eliminar la entrada del calendario.");
                }
            }
            else
            {
                MessageBox.Show("El objeto pasado como parámetro no es una entrada del calendario.");
            }
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
