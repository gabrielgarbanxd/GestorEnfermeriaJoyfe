using GestorEnfermeriaJoyfe.ApplicationLayer.VisitTemplateApp;
using GestorEnfermeriaJoyfe.Domain.VisitTemplate;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Adapters.VisitTemplateAdapters
{
    public class VisitTemplateCommandAdapter : CommandAdapterBase
    {
        private readonly IVisitTemplateContract visitTemplateRepository;

        public VisitTemplateCommandAdapter(IVisitTemplateContract visitTemplateRepository) => this.visitTemplateRepository = visitTemplateRepository;

        public async Task<CommandResponse> CreateVisitTemplate(VisitTemplate visitTemplate)
        {
            return await RunCommand(async () =>
            {
                return await new VisitTemplateCreator(visitTemplateRepository).Run(visitTemplate);
            });
        }

        public async Task<CommandResponse> UpdateVisitTemplate(VisitTemplate visitTemplate)
        {
            return await RunCommand(async () =>
            {
                return await new VisitTemplateUpdater(visitTemplateRepository).Run(visitTemplate);
            });
        }

        public async Task<CommandResponse> DeleteVisitTemplate(int id)
        {
            return await RunCommand(async () =>
            {
                return await new VisitTemplateDeleter(visitTemplateRepository).Run(id);
            });
        }

       
    }
}
