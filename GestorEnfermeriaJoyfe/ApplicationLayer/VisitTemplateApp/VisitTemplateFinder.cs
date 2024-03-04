using GestorEnfermeriaJoyfe.Domain.Visit;
using GestorEnfermeriaJoyfe.Domain.Visit.ValueObjects;
using GestorEnfermeriaJoyfe.Domain.VisitTemplate;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.ApplicationLayer.VisitTemplateApp
{
    public class VisitTemplateFinder
    {
        private readonly IVisitTemplateContract _visitTemplateRepository;

        public VisitTemplateFinder(IVisitTemplateContract visitTemplateContract) => _visitTemplateRepository = visitTemplateContract;

        public async Task<VisitTemplate> Run(int id) => await _visitTemplateRepository.FindAsync(new(id));
    }
}
