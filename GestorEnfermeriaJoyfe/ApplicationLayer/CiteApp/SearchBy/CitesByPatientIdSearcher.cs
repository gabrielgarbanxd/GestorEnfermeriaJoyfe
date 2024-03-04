using GestorEnfermeriaJoyfe.Domain.Cite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.ApplicationLayer.CiteApp.SearchBy
{
    public class CitesByPatientIdSearcher
    {
        private readonly ICiteContract _citeContract;

        public CitesByPatientIdSearcher(ICiteContract citeContract) => _citeContract = citeContract;

        public async Task<IEnumerable<Cite>> Run(int patientId, bool paginated = false, int perPage = 10, int page = 1)
        {
            if (paginated)
            {
                if (perPage <= 0 || page <= 0)
                {
                    throw new ArgumentException("El número de página y el número de elementos por página deben ser mayores a 0.");
                }

                return await _citeContract.GetCitesByPatientIdAsync(patientId, paginated, perPage, page);
            }

            return await _citeContract.GetCitesByPatientIdAsync(patientId);
        }
    }
}
