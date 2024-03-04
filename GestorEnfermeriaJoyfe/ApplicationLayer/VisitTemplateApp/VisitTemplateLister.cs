using GestorEnfermeriaJoyfe.Domain.Visit;
using GestorEnfermeriaJoyfe.Domain.VisitTemplate;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.ApplicationLayer.VisitTemplateApp
{
    public class VisitTemplateLister
    {
        private readonly IVisitTemplateContract _visitTemplateRepository;

        public VisitTemplateLister(IVisitTemplateContract visitTemplateContract) => _visitTemplateRepository = visitTemplateContract;

        public async Task<IEnumerable<VisitTemplate>> Run(bool paginated = false, int perPage = 10, int page = 1)
        {
            if (paginated)
            {
                if (perPage <= 0 || page <= 0)
                {
                    throw new ArgumentException("El número de página y el número de elementos por página deben ser mayores a 0.");
                }

                return await _visitTemplateRepository.GetAllAsync(paginated, perPage, page);
            }

            return await _visitTemplateRepository.GetAllAsync();
        }
    }
}
