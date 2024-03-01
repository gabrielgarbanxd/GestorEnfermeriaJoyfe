using GestorEnfermeriaJoyfe.Domain.Cite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.ApplicationLayer.CiteApp
{
    internal class CiteFinder
    {
        private readonly ICiteContract _citeRepository;

        public CiteFinder(ICiteContract citeRepository) => _citeRepository = citeRepository;

        public async Task<Cite> Run(int id) => await _citeRepository.FindAsync(new Domain.Cite.ValueObjects.CiteId(id));

    }
}
