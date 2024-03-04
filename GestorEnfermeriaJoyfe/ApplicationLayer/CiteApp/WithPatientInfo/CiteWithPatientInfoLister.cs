using GestorEnfermeriaJoyfe.Domain.Cite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.ApplicationLayer.CiteApp.WithPatientInfo
{
    public class CiteWithPatientInfoLister
    {
        private readonly ICiteContract citeContract;

        public CiteWithPatientInfoLister(ICiteContract citeContract)
        {
            this.citeContract = citeContract;
        }

        public async Task<IEnumerable<Cite>> Run(bool paginated = false, int perPage = 10, int page = 1)
        {
            if (paginated)
            {
                if (perPage <= 0 || page <= 0)
                {
                    throw new ArgumentException("El número de página y el número de elementos por página deben ser mayores a 0.");
                }

                return await citeContract.GetAlWithPatientInfoAsync(paginated, perPage, page);
            }

            return await citeContract.GetAlWithPatientInfoAsync();
        }
    }
}
