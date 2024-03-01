using GestorEnfermeriaJoyfe.ApplicationLayer.CiteApp;
using GestorEnfermeriaJoyfe.Adapters.CiteAdapters;
using GestorEnfermeriaJoyfe.Domain.Cite;
using System.Collections.Generic;
using System.Threading.Tasks;



namespace GestorEnfermeriaJoyfe.Adapters.CiteAdapters
{
    public class CiteQueryAdapter : QueryAdapterBase<CiteResponse, IEnumerable<Cite>>
    {
        private readonly ICiteContract citeRepository;

        public CiteQueryAdapter(ICiteContract citeRepository, CiteResponse response) : base(response)
        {
            this.citeRepository = citeRepository;
        }

        public async Task<CiteResponse> FindCite(int id)
        {
            return await RunQuery(async () =>
            {
                return new List<Cite> { await new CiteFinder(citeRepository).Run(id) };
            });
        }

        public async Task<CiteResponse> GetAllCites()
        {
            return await RunQuery(async () =>
            {
                return await new CiteLister(citeRepository).Run();
            });
        }

        public async Task<CiteResponse> GetAllCitesPaginated(int perPage, int page)
        {
            return await RunQuery(async () =>
            {
                return await new CiteLister(citeRepository).Run(true, perPage, page);
            });
        }
    }
}
