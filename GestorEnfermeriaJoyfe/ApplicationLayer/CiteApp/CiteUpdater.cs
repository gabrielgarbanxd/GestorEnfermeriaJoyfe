using GestorEnfermeriaJoyfe.Domain.Cite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.ApplicationLayer.CiteApp
{
    internal class CiteUpdater
    {
        public readonly ICiteContract _citeRepository;

        public CiteUpdater(ICiteContract citeRepository) => _citeRepository = citeRepository;

        public async Task<int> Run(Cite cite) => await _citeRepository.UpdateAsync(cite);

    }
}

