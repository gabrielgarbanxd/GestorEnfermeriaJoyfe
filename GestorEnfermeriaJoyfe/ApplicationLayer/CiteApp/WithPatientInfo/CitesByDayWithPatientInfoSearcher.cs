using GestorEnfermeriaJoyfe.Domain.Cite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.ApplicationLayer.CiteApp.WithPatientInfo
{
    public class CitesByDayWithPatientInfoSearcher
    {
        private readonly ICiteContract _citeContract;

        public CitesByDayWithPatientInfoSearcher(ICiteContract citeContract)
        {
            this._citeContract = citeContract;
        }

        public async Task<IEnumerable<Cite>> Run(DateTime date, bool paginated = false, int perPage = 10, int page = 1)
        {
            if (paginated)
            {
                if (perPage <= 0 || page <= 0)
                {
                    throw new ArgumentException("El número de página y el número de elementos por página deben ser mayores a 0.");
                }

                return await _citeContract.GetCitesByDayWithPatientInfoAsync(date, paginated, perPage, page);
            }

            return await _citeContract.GetCitesByDayWithPatientInfoAsync(date);
        }
    }
}
