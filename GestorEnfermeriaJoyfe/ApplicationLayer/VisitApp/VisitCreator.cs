using GestorEnfermeriaJoyfe.Domain.Visit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.ApplicationLayer.VisitApp
{
    public class VisitCreator
    {
        private readonly IVisitContract _visitRepository;

        public VisitCreator(IVisitContract visitRepository) => _visitRepository = visitRepository;

        public async Task<int> Run(Visit visit) => await _visitRepository.CreateAsync(visit);
    }
}
