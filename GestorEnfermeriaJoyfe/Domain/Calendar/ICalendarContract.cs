using GestorEnfermeriaJoyfe.Domain.Calendar.ValueObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Domain.Calendar
{
    public interface ICalendarContract
    {
        Task<IEnumerable<Calendar>> GetAllAsync(bool paginated = false, int perPage = 10, int page = 1);
        Task<Calendar> FindAsync(CalendarId id);
        Task<int> AddAsync(Calendar calendarEvent);
        Task<int> UpdateAsync(Calendar calendarEvent);
        Task<int> DeleteAsync(CalendarId id);
    }
}
