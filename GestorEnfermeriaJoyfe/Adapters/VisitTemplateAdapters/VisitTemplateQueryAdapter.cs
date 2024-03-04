using GestorEnfermeriaJoyfe.ApplicationLayer.VisitTemplateApp;
using GestorEnfermeriaJoyfe.Domain.VisitTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Adapters.VisitTemplateAdapters
{
    public class VisitTemplateQueryAdapter : QueryAdapterBase<VisitTemplateResponse, IEnumerable<VisitTemplate>>
    {
        private readonly IVisitTemplateContract visitTemplateRepository;

        public VisitTemplateQueryAdapter(IVisitTemplateContract visitTemplateRepository, VisitTemplateResponse response) : base(response)
        {
            this.visitTemplateRepository = visitTemplateRepository;
        }

        public async Task<VisitTemplateResponse> FindVisitTemplate(int id)
        {
            return await RunQuery(async () =>
            {
                return new List<VisitTemplate> { await new VisitTemplateFinder(visitTemplateRepository).Run(id) };
            });
        }

        public async Task<VisitTemplateResponse> GetAllVisitTemplates()
        {
            return await RunQuery(async () =>
            {
                return await new VisitTemplateLister(visitTemplateRepository).Run();
            });
        }

        public async Task<VisitTemplateResponse> GetAllVisitTemplatesPaginated(int perPage, int page)
        {
            return await RunQuery(async () =>
            {
                return await new VisitTemplateLister(visitTemplateRepository).Run(true, perPage, page);
            });
        }
    }
}
