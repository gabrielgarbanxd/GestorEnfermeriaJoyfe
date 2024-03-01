using GestorEnfermeriaJoyfe.Adapters.PatientAdapters;
using GestorEnfermeriaJoyfe.ApplicationLayer.CiteApp;
using GestorEnfermeriaJoyfe.Domain.Cite;
using GestorEnfermeriaJoyfe.Infraestructure.CitePersistence;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Adapters.CiteAdapters
{
    public class CiteController
    {
        private readonly CiteMapper citeMapper;
        private readonly MySqlCiteRepository citeRepository;

        private readonly CiteQueryAdapter CiteQueryAdapter;
        private readonly CiteCommandAdapter CiteCommandAdapter;


        public CiteController()
        {
            citeMapper = new ();
            citeRepository = new (citeMapper);


            CiteQueryAdapter = new(citeRepository, new());
            CiteCommandAdapter = new(citeRepository);
        }

        // ================== QUERYS ==================

        public async Task<CiteResponse> GetAll() => await CiteQueryAdapter.GetAllCites();
        public async Task<CiteResponse> GetAllPaginated(int perPage, int page) => await CiteQueryAdapter.GetAllCitesPaginated(perPage, page);
        public async Task<CiteResponse> Get(int id) => await CiteQueryAdapter.FindCite(id);

        // ================== COMMANDS ==================
        public async Task<CommandResponse> Create(Cite cite) => await CiteCommandAdapter.Create(cite);
        public async Task<CommandResponse> Update(Cite cite) => await CiteCommandAdapter.Update(cite);
        public async Task<CommandResponse> Delete(int id) => await CiteCommandAdapter.Delete(id);
    }
}
