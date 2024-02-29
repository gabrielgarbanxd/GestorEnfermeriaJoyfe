using GestorEnfermeriaJoyfe.Domain.Calendar;
using GestorEnfermeriaJoyfe.Domain.Cites;
using System;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.ApplicationLayer.CitesApp
{
    public class CitesCreator
    {
        private readonly ICitesContract _citesRepository;

        public CitesCreator(ICitesContract citesRepository)
        {
            _citesRepository = citesRepository ?? throw new ArgumentNullException(nameof(citesRepository));
        }

        public async Task<int> Run(Cite cite) => await _citesRepository.CreateAsync(cite);
    }
}
