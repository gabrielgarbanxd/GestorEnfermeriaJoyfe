using GestorEnfermeriaJoyfe.Domain.VisitTemplate;
using GestorEnfermeriaJoyfe.Infraestructure.VisitTemplatePersistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Adapters.VisitTemplateAdapters
{
    public class VisitTemplateController
    {
        private readonly VisitTemplateMapper visitTemplateMapper;
        private readonly MySqlVisitTemplateRepository visitTemplateRepository;

        private readonly VisitTemplateQueryAdapter visitTemplateQueryAdapter;
        private readonly VisitTemplateCommandAdapter visitTemplateCommandAdapter;

        public VisitTemplateController()
        {
            visitTemplateMapper = new VisitTemplateMapper();
            visitTemplateRepository = new(visitTemplateMapper);

            visitTemplateQueryAdapter = new(visitTemplateRepository, new());
            visitTemplateCommandAdapter = new(visitTemplateRepository);
        }

        // ================== QUERYS ==================

        public async Task<VisitTemplateResponse> GerAll() => await visitTemplateQueryAdapter.GetAllVisitTemplates();
        public async Task<VisitTemplateResponse> GetAllPaginated(int perPage, int page) => await visitTemplateQueryAdapter.GetAllVisitTemplatesPaginated(perPage, page);
        public async Task<VisitTemplateResponse> Get(int id) => await visitTemplateQueryAdapter.FindVisitTemplate(id);

        // ================== COMMANDS ==================

        public async Task<CommandResponse> Create(VisitTemplate visitTemplate) => await visitTemplateCommandAdapter.CreateVisitTemplate(visitTemplate);
        public async Task<CommandResponse> Update(VisitTemplate visitTemplate) => await visitTemplateCommandAdapter.UpdateVisitTemplate(visitTemplate);
        public async Task<CommandResponse> Delete(int id) => await visitTemplateCommandAdapter.DeleteVisitTemplate(id);
    }
}
