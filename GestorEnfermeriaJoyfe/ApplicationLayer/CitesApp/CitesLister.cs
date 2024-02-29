using GestorEnfermeriaJoyfe.Domain.Calendar;
using GestorEnfermeriaJoyfe.Domain.Cites;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.ApplicationLayer.CitesApp
{
    public class CitesLister
    {
        private readonly ICitesContract _citesRepository;

        public CitesLister(ICitesContract citesRepository)
        {
            _citesRepository = citesRepository;
        }

        public async Task<IEnumerable<Cite>> Run(bool paginated = false, int perPage = 10, int page = 1)
        {
            if (paginated)
            {
                if (perPage <= 0 || page <= 0)
                {
                    throw new System.ArgumentException("El número de página y el número de elementos por página deben ser mayores a 0.");
                }

                return await _citesRepository.GetAllAsync(paginated, perPage, page);
            }

            return await _citesRepository.GetAllAsync();
        }
    }
}
