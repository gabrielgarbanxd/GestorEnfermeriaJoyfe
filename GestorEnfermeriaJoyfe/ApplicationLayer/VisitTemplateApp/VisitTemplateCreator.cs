using GestorEnfermeriaJoyfe.Domain.Visit;
using GestorEnfermeriaJoyfe.Domain.VisitTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.ApplicationLayer.VisitTemplateApp
{
    public class VisitTemplateCreator
    {
        private readonly IVisitTemplateContract _visitTemplateRepository;

        public VisitTemplateCreator(IVisitTemplateContract visitTemplateContract) => _visitTemplateRepository = visitTemplateContract;

        public async Task<int> Run(VisitTemplate visit) => await _visitTemplateRepository.CreateAsync(visit);
    }
}
