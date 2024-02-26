using GestorEnfermeriaJoyfe.Domain.Visit;
using GestorEnfermeriaJoyfe.Domain.Visit.ValueObjects;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.ApplicationLayer.VisitApp
{
    public class VisitFinder
    {
        private readonly IVisitContract _visitRepository;

        public VisitFinder(IVisitContract visitRepository) => _visitRepository = visitRepository;

        public async Task<Visit> Run(int id) => await _visitRepository.FindAsync(new VisitId(id));
    }
}
