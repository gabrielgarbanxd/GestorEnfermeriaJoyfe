using GestorEnfermeriaJoyfe.Domain.Cite.ValueObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Domain.Cite
{
    public interface ICiteContract
    {
        Task<IEnumerable<Cite>> GetAllAsync(bool paginated = false, int perPage = 10, int page = 1);
        Task<Cite> FindAsync(CiteId citeId);
        Task<int> AddAsync(Cite cite);
        Task<int> UpdateAsync(Cite cite);
        Task<int> DeleteAsync(CiteId citeId);
    }
}
