using GestorEnfermeriaJoyfe.ApplicationLayer.CitesApp;
using GestorEnfermeriaJoyfe.Domain.Cites;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Adapters.CitesAdapters
{
    public class CitesQueryAdapter : QueryAdapterBase<CitesResponse, IEnumerable<Cite>>
    {
        private readonly ICitesContract citesRepository;

        public CitesQueryAdapter(ICitesContract citesRepository, CitesResponse response) : base(response)
        {
            this.citesRepository = citesRepository;
        }

        public async Task<CitesResponse> FindCite(int id)
        {
            return await RunQuery(async () =>
            {
                return new List<Cite> { await new CitesFinder(citesRepository).Run(id) };
            });
        }

        public async Task<CitesResponse> GetAllCites()
        {
            return await RunQuery(async () =>
            {
                return await new CitesLister(citesRepository).Run();
            });
        }

        public async Task<CitesResponse> GetAllCitesPaginated(int perPage, int page)
        {
            return await RunQuery(async () =>
            {
                return await new CitesLister(citesRepository).Run(true, perPage, page);
            });
        }
    }
}
