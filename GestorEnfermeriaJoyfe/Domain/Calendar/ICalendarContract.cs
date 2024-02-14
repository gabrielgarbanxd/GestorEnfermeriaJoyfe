using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Domain.Calendar
{
    public interface ICalendarContract
    {
        Task<List<Calendar>> GetAllAsync();
        Task<Calendar> FindAsync(int eventId);
        Task<int> AddAsync(Calendar calendarEvent);
        Task<int> UpdateAsync(Calendar calendarEvent);
        Task<int> DeleteAsync(int eventId);
    }
}
