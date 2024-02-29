using GestorEnfermeriaJoyfe.Domain.Calendar;
using GestorEnfermeriaJoyfe.Domain.Cites;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.ApplicationLayer.CitesApp
{
    public class CitesFinder
    {
        private readonly ICitesContract _citesRepository;

        public CitesFinder(ICitesContract citesRepository)
        {
            _citesRepository = citesRepository;
        }

        public async Task<Cite> Run(int id) => await _citesRepository.FindAsync(id);
    }
}
