using System;
using System.Security.Policy;
using System.Threading.Tasks;
using GestorEnfermeriaJoyfe.ApplicationLayer.CitesApp;
using GestorEnfermeriaJoyfe.Domain.Cites;

namespace GestorEnfermeriaJoyfe.Adapters.CitesAdapters
{
    public class CitesCommandAdapter : CommandAdapterBase
    {
        private readonly ICitesContract citesRepository;

        public CitesCommandAdapter(ICitesContract citesRepository)
        {
            this.citesRepository = citesRepository ?? throw new ArgumentNullException(nameof(citesRepository));
        }

        public async Task<CommandResponse> CreateCite(Cite cite)
        {
            return await RunCommand(async () =>
            {
                return await new CitesCreator(citesRepository).Run(cite);
            });
        }

        public async Task<CommandResponse> UpdateCite(Cite cite)
        {
            return await RunCommand(async () =>
            {
                return await new CitesUpdater(citesRepository).Run(cite);
            });
        }

        public async Task<CommandResponse> DeleteCite(int id)
        {
            return await RunCommand(async () =>
            {
                return await new CitesDeleter(citesRepository).Run(id);
            });
        }
    }
}
