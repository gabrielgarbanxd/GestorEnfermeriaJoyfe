using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using GestorEnfermeriaJoyfe.Domain.Calendar;
using GestorEnfermeriaJoyfe.Domain.Calendar.ValueObjects;
using GestorEnfermeriaJoyfe.Infraestructure.Shared;

namespace GestorEnfermeriaJoyfe.Infraestructure.CalendarPersistence
{
    public class MySqlCalendarRepository : MySqlRepositoryBase<Calendar>, ICalendarContract
    {
        public MySqlCalendarRepository(CalendarMapper mapper) : base(mapper)
        {
        }

        public MySqlCalendarRepository()
        {
        }

        public Task<IEnumerable<Calendar>> GetAllAsync(bool paginated = false, int perPage = 10, int page = 1)
        {
            throw new NotImplementedException();
        }

        public Task<Calendar> FindAsync(CalendarId id)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddAsync(Calendar calendarEvent)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(Calendar calendarEvent)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(CalendarId id)
        {
            throw new NotImplementedException();
        }
    }
}
