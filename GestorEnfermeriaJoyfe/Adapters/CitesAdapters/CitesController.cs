using GestorEnfermeriaJoyfe.Domain.Cites;
using GestorEnfermeriaJoyfe.Adapters.CitesAdapters;
using GestorEnfermeriaJoyfe.Infraestructure.CitesPersistence;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Adapters.CitesAdapters
{
    public class CitesController
    {
        private readonly CitesMapper _citesMapper;
        private readonly MySqlCitesRepository _citesRepository;
        private readonly CitesQueryAdapter _citesQueryAdapter;
        private readonly CitesCommandAdapter _citesCommandAdapter;

        public CitesController()
        {
            _citesMapper = new CitesMapper();
            _citesRepository = new MySqlCitesRepository(_citesMapper);
            var citesResponse = new CitesResponse(); // Crear una instancia de CitesResponse
            _citesQueryAdapter = new CitesQueryAdapter(_citesRepository, citesResponse); // Proporcionar citesResponse al constructor
            _citesCommandAdapter = new CitesCommandAdapter(_citesRepository);
        }

        // ================== QUERYS ==================

        public async Task<CitesResponse> GetAll() => await _citesQueryAdapter.GetAllCites();
        public async Task<CitesResponse> GetAllPaginated(int perPage, int page) => await _citesQueryAdapter.GetAllCitesPaginated(perPage, page);
        public async Task<CitesResponse> Get(int id) => await _citesQueryAdapter.FindCite(id);

        // ================== COMMANDS ==================

        public async Task<CommandResponse> Create(Cite cite) => await _citesCommandAdapter.CreateCite(cite);
        public async Task<CommandResponse> Update(Cite cite) => await _citesCommandAdapter.UpdateCite(cite);
        public async Task<CommandResponse> Delete(int id) => await _citesCommandAdapter.DeleteCite(id);
    }
}
