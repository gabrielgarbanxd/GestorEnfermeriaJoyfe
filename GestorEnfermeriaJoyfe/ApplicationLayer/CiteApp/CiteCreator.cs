using GestorEnfermeriaJoyfe.Domain.Cite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.ApplicationLayer.CiteApp
{
    internal class CiteCreator
    {
        private readonly ICiteContract _citeRpository;

        public CiteCreator(ICiteContract citeRepository) => _citeRpository = citeRepository;

        public async Task<int> Run(Cite cite) => await _citeRpository.AddAsync(cite);
    }
}
