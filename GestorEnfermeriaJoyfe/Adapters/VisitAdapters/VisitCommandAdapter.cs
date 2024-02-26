using GestorEnfermeriaJoyfe.ApplicationLayer.VisitApp;
using GestorEnfermeriaJoyfe.Domain.Visit;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Adapters.VisitAdapters
{
    public class VisitCommandAdapter : CommandAdapterBase
    {
        private readonly IVisitContract visitRepository;

        public VisitCommandAdapter(IVisitContract visitRepository) => this.visitRepository = visitRepository;

        public async Task<CommandResponse> CreateVisit(Visit visit)
        {
            return await RunCommand(async () =>
            {
                return await new VisitCreator(visitRepository).Run(visit);
            });
        }

        public async Task<CommandResponse> UpdateVisit(Visit visit)
        {
            return await RunCommand(async () =>
            {
                return await new VisitUpdater(visitRepository).Run(visit);
            });
        }

        public async Task<CommandResponse> DeleteVisit(int id)
        {
            return await RunCommand(async () =>
            {
                return await new VisitDeleter(visitRepository).Run(id);
            });
        }
    }
}
