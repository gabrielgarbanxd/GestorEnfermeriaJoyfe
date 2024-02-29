using GestorEnfermeriaJoyfe.Domain.Cites;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.ApplicationLayer.CitesApp
{
    public class CitesDeleter
    {
        private readonly ICitesContract _citesRepository;

        public CitesDeleter(ICitesContract citesRepository)
        {
            _citesRepository = citesRepository;
        }

        public async Task<int> Run(int id) => await _citesRepository.DeleteAsync(id);
    }
}
