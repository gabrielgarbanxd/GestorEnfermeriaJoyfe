using GestorEnfermeriaJoyfe.Domain.VisitTemplate;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.ApplicationLayer.VisitTemplateApp
{
    public class VisitTemplateUpdater
    {
        private readonly IVisitTemplateContract _visitTemplateRepository;

        public VisitTemplateUpdater(IVisitTemplateContract visitTemplateContract) => _visitTemplateRepository = visitTemplateContract;

        public async Task<int> Run(VisitTemplate visitTemplate) => await _visitTemplateRepository.UpdateAsync(visitTemplate);
    }
}
