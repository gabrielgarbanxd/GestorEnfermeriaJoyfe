using GestorEnfermeriaJoyfe.Domain.Calendar;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Domain.Cites
{
    public interface ICitesContract
    {
        Task<IEnumerable<Cite>> GetAllAsync(bool paginated = false, int perPage = 10, int page = 1);
        Task<Cite> FindAsync(int citeId);
        Task<int> CreateAsync(Cite cite);
        Task<int> UpdateAsync(Cite cite);
        Task<int> DeleteAsync(int citeId);
    }
}
