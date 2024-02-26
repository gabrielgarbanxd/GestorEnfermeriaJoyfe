using GestorEnfermeriaJoyfe.Domain.Visit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.ApplicationLayer.VisitApp
{
    public class VisitUpdater
    {
        private readonly IVisitContract _visitRepository;

        public VisitUpdater(IVisitContract visitContract) => _visitRepository = visitContract;

        public async Task<int> Run(Visit visit) => await _visitRepository.UpdateAsync(visit);

    }
}
