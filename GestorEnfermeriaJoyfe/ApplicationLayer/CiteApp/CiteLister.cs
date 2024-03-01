using GestorEnfermeriaJoyfe.Domain.Cite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.ApplicationLayer.CiteApp
{
    internal class CiteLister
    {
        private readonly ICiteContract _citeRepository;

        public CiteLister(ICiteContract citeRepository) => _citeRepository = citeRepository;

        public async Task<IEnumerable<Cite>> Run(bool paginated = false, int perPage = 10, int page = 1)
        {
            if (paginated)
            {
                if (perPage <= 0 || page <= 0)
                {
                    throw new System.ArgumentException("El número de página y el número de elementos por página deben ser mayores a 0.");
                }

                return await _citeRepository.GetAllAsync(paginated, perPage, page);
            }

            return await _citeRepository.GetAllAsync();
        }
    }
}
