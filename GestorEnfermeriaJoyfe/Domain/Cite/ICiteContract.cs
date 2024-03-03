using GestorEnfermeriaJoyfe.Domain.Cite.ValueObjects;
using System;
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



        Task<IEnumerable<Cite>> GetCitesByPatientIdAsync(int patientId, bool paginated = false, int perPage = 10, int page = 1);
        Task<IEnumerable<Cite>> GetCitesByDayAsync(DateTime date, bool paginated = false, int perPage = 10, int page = 1);
        Task<IEnumerable<Cite>> GetCitesByDayAndPatientIdAsync(int patientId, DateTime date, bool paginated = false, int perPage = 10, int page = 1);
        Task<IEnumerable<Cite>> GetCitesByDayRangeAsync(DateTime startDate, DateTime endDate, bool paginated = false, int perPage = 10, int page = 1);
        Task<IEnumerable<Cite>> GetCitesByPatientIdAndDayAsync(int patientId, DateTime date, bool paginated = false, int perPage = 10, int page = 1);
        Task<IEnumerable<Cite>> GetCitesByPatientIdAndDayRangeAsync(int patientId, DateTime startDate, DateTime endDate, bool paginated = false, int perPage = 10, int page = 1);





    }
}

