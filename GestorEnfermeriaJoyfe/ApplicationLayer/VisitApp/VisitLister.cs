using GestorEnfermeriaJoyfe.Domain.Visit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.ApplicationLayer.VisitApp
{
    public class VisitLister
    {
        private readonly IVisitContract _visitRepository;

        public VisitLister(IVisitContract visitRepository) => _visitRepository = visitRepository;

        public async Task<IEnumerable<Visit>> Run(bool paginated = false, int perPage = 10, int page = 1)
        {
            if (paginated)
            {
                if (perPage <= 0 || page <= 0)
                {
                    throw new ArgumentException("El número de página y el número de elementos por página deben ser mayores a 0.");
                }

                return await _visitRepository.GetAllAsync(paginated, perPage, page);
            }

            return await _visitRepository.GetAllAsync();
        }
    }
}
