using GestorEnfermeriaJoyfe.Domain.Visit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.ApplicationLayer.VisitApp.SearchBy
{
    public class VisitsByDateRangeSearcher
    {
        private readonly IVisitContract _visitContract;

        public VisitsByDateRangeSearcher(IVisitContract visitContract) => _visitContract = visitContract;

        public async Task<IEnumerable<Visit>> Run(DateTime startDate, DateTime endDate, bool paginated = false, int perPage = 10, int page = 1)
        {
            if (paginated)
            {
                if (perPage <= 0 || page <= 0)
                {
                    throw new ArgumentException("El número de página y el número de elementos por página deben ser mayores a 0.");
                }

                return await _visitContract.SearchByDateRangeAsync(startDate, endDate, paginated, perPage, page);
            }

            return await _visitContract.SearchByDateRangeAsync(startDate, endDate);
        }
    }
}
