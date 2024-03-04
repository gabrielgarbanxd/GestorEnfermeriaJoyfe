using GestorEnfermeriaJoyfe.Domain.Visit;
using GestorEnfermeriaJoyfe.Domain.Visit.ValueObjects;
using GestorEnfermeriaJoyfe.Domain.VisitTemplate;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.ApplicationLayer.VisitTemplateApp
{
    public class VisitTemplateDeleter
    {
        private readonly IVisitTemplateContract _visitTemplateRepository;

        public VisitTemplateDeleter(IVisitTemplateContract visitTemplateContract) => _visitTemplateRepository = visitTemplateContract;

        public async Task<int> Run(int id) => await _visitTemplateRepository.DeleteAsync(new(id));
    }
}
