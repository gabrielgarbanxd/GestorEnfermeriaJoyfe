using GestorEnfermeriaJoyfe.Domain.Visit;
using GestorEnfermeriaJoyfe.Domain.Visit.ValueObjects;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.ApplicationLayer.VisitApp
{
    public class VisitDeleter
    {
        private readonly IVisitContract _visitContract;

        public VisitDeleter(IVisitContract visitContract) => _visitContract = visitContract;

        public async Task<int> Run(int id) => await _visitContract.DeleteAsync(new VisitId(id));
    }
}
