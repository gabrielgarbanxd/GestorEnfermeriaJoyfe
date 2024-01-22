using System;
using System.Windows.Input;

namespace GestorEnfermeriaJoyfe.UI.ViewModels
{
    /// <summary>
    /// Clase que implementa la interfaz ICommand para encapsular la lógica de ejecución de comandos.
    /// </summary>
    public class ViewModelCommand : ICommand
    {
        // Campos (Fields)
        private readonly Action<object> _executeAction;           // Acción a ejecutar cuando se llama al comando.
        private readonly Predicate<object> _canExecuteAction;    // Condición para determinar si el comando puede ejecutarse.

        // Constructores

        /// <summary>
        /// Constructor que toma una acción a ejecutar.
        /// </summary>
        /// <param name="executeAction">Acción a ejecutar cuando se llama al comando.</param>
        public ViewModelCommand(Action<object> executeAction)
        {
            _executeAction = executeAction;
            _canExecuteAction = null;   // No hay condición de ejecución, siempre puede ejecutarse.
        }

        /// <summary>
        /// Constructor que toma una acción a ejecutar y una condición para determinar si el comando puede ejecutarse.
        /// </summary>
        /// <param name="executeAction">Acción a ejecutar cuando se llama al comando.</param>
        /// <param name="canExecuteAction">Condición para determinar si el comando puede ejecutarse.</param>
        public ViewModelCommand(Action<object> executeAction, Predicate<object> canExecuteAction)
        {
            _executeAction = executeAction;
            _canExecuteAction = canExecuteAction;
        }

        // Eventos

        /// <summary>
        /// Evento que se dispara cuando cambia la capacidad de ejecución del comando.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            // Agrega o quita controladores de eventos que reevalúan si el comando puede ejecutarse.
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        // Métodos

        /// <summary>
        /// Determina si el comando puede ejecutarse en el estado actual.
        /// </summary>
        /// <param name="parameter">Parámetro del comando.</param>
        /// <returns>Devuelve true si el comando puede ejecutarse, false de lo contrario.</returns>
        public bool CanExecute(object parameter)
        {
            // Si no se proporciona una condición de ejecución, el comando siempre puede ejecutarse.
            return _canExecuteAction == null ? true : _canExecuteAction(parameter);
        }

        /// <summary>
        /// Ejecuta la lógica asociada al comando.
        /// </summary>
        /// <param name="parameter">Parámetro del comando.</param>
        public void Execute(object parameter)
        {
            // Ejecuta la acción asociada al comando.
            _executeAction(parameter);
        }
    }
}
