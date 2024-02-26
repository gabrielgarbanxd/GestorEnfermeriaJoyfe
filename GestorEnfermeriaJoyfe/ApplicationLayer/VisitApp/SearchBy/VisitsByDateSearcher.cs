using GestorEnfermeriaJoyfe.Domain.Visit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.ApplicationLayer.VisitApp.SearchBy
{
    public class VisitsByDateSearcher
    {
        private readonly IVisitContract _visitContract;

        public VisitsByDateSearcher(IVisitContract visitContract) => _visitContract = visitContract;

        public async Task<IEnumerable<Visit>> Run(DateTime date, bool paginated = false, int perPage = 10, int page = 1)
        {
            if (paginated)
            {
                if (perPage <= 0 || page <= 0)
                {
                    throw new ArgumentException("El número de página y el número de elementos por página deben ser mayores a 0.");
                }

                return await _visitContract.SearchByDateAsync(date, paginated, perPage, page);
            }

            return await _visitContract.SearchByDateAsync(date);
        }
    }
}
