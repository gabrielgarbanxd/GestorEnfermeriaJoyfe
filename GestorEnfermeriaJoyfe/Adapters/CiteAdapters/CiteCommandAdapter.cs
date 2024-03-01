using GestorEnfermeriaJoyfe.ApplicationLayer.CiteApp;
using GestorEnfermeriaJoyfe.Domain.Cite;
using GestorEnfermeriaJoyfe.Infraestructure.CitePersistence;
using System;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Adapters.CiteAdapters
{
    public class CiteCommandAdapter : CommandAdapterBase
    {
        private readonly ICiteContract citeRepository;

        public CiteCommandAdapter(ICiteContract citeRepository) => this.citeRepository = citeRepository;


        public async Task<CommandResponse> Create(Cite cite)
        {
            return await RunCommand(async () =>
            {
                return await new CiteCreator(citeRepository).Run(cite);
            });
        }

        public async Task<CommandResponse> Update(Cite cite)
        {
            return await RunCommand(async () =>
            {
                return await new CiteUpdater(citeRepository).Run(cite);
            });
        }


        public async Task<CommandResponse> Delete(int id)
        {
            return await RunCommand(async () =>
            {
                return await new CiteDeleter(citeRepository).Run(id);
            });
        }
    }
}
