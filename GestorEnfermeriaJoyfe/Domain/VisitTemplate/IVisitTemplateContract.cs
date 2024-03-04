using GestorEnfermeriaJoyfe.Domain.VisitTemplate.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Domain.VisitTemplate
{
    public interface IVisitTemplateContract
    {
        Task<IEnumerable<VisitTemplate>> GetAllAsync(bool paginated = false, int perPage = 10, int page = 1);
        Task<VisitTemplate> FindAsync(VisitTemplateId visitTemplateId);
        Task<int> CreateAsync(VisitTemplate visitTemplate);
        Task<int> UpdateAsync(VisitTemplate visitTemplate);
        Task<int> DeleteAsync(VisitTemplateId visitTemplateId);
    }
}
