using GestorEnfermeriaJoyfe.Domain.Cite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestorEnfermeriaJoyfe.Domain.Cite.ValueObjects;


namespace GestorEnfermeriaJoyfe.ApplicationLayer.CiteApp
{
    internal class CiteDeleter
    {
        private readonly ICiteContract _citeRepository;

        public CiteDeleter(ICiteContract citeRepository) => _citeRepository = citeRepository;

        public async Task<int> Run(int id) => await _citeRepository.DeleteAsync(new CiteId(id));

    }
}

