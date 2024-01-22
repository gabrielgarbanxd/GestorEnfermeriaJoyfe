using System.ComponentModel;

namespace GestorEnfermeriaJoyfe.UI.ViewModels
{
    /// Clase base abstracta para ViewModels que implementa la interfaz INotifyPropertyChanged.
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        /// Evento que se dispara cuando una propiedad cambia.

        public event PropertyChangedEventHandler? PropertyChanged;

        /// Método para invocar el evento PropertyChanged y notificar cambios en una propiedad específica.

        /// <param name="propertyName"></param>
        public void OnPropertyChanged(string propertyName)
        {

            // Invoca el evento PropertyChanged si hay controladores registrados.

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));  


        }


    }
}
