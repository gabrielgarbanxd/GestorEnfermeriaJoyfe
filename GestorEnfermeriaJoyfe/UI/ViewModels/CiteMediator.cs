using GestorEnfermeriaJoyfe.Domain.Cite;
using System;

namespace GestorEnfermeriaJoyfe.UI.ViewModels
{
    public class CiteMediator
    {
        // Singleton instance
        public static readonly CiteMediator Instance = new CiteMediator();
        private CiteMediator() { }

        // Eventos para citas
        public event Action<Cite> CiteUpdated;
        public event Action<Cite> CiteDeleted;
        public event Action<Cite> CiteCreated;

        // Métodos para invocar los eventos
        public void OnCiteUpdated(Cite updatedCite)
        {
            CiteUpdated?.Invoke(updatedCite);
        }

        public void OnCiteDeleted(Cite deletedCite)
        {
            CiteDeleted?.Invoke(deletedCite);
        }

        public void OnCiteCreated(Cite newCite)
        {
            CiteCreated?.Invoke(newCite);
        }
    }
}
