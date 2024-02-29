using GestorEnfermeriaJoyfe.Domain.Calendar;
using GestorEnfermeriaJoyfe.Domain.Cites;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.ApplicationLayer.CitesApp
{
    public class CitesUpdater
    {
        private readonly ICitesContract _citesRepository;

        public CitesUpdater(ICitesContract citesRepository)
        {
            _citesRepository = citesRepository;
        }

        public async Task<int> Run(Cite cite) => await _citesRepository.UpdateAsync(cite);
    }
}
