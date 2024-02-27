using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<int> AddAsync(Calendar calendarEvent)
        {
            var parameters = new Dictionary<string, object>
            {
                {"p_date", calendarEvent.Date},
                {"p_task", calendarEvent.Task}
            };
            return await ExecuteNonQueryAsync("CreateCalendarEventProcedure", parameters);
        }

        public async Task<int> UpdateAsync(Calendar calendarEvent)
        {
            var parameters = new Dictionary<string, object>
            {
                {"p_id", calendarEvent.Id.Value},
                {"p_date", calendarEvent.Date},
                {"p_task", calendarEvent.Task}
            };
            return await ExecuteNonQueryAsync("UpdateCalendarEventProcedure", parameters);
        }

        public async Task<int> DeleteAsync(CalendarId id)
        {
            var parameters = new Dictionary<string, object>
            {
                {"p_id", id.Value}
            };
            return await ExecuteNonQueryAsync("DeleteCalendarEventProcedure", parameters);
        }

        public async Task<Calendar> FindAsync(CalendarId id)
        {
            var result = await ExecuteQueryAsync("GetCalendarEventByIdProcedure", new Dictionary<string, object> { { "p_id", id.Value } });

            if (!result.Any())
            {
                throw new Exception("No se ha encontrado el evento del calendario.");
            }

            return result.First();
        }

        public async Task<IEnumerable<Calendar>> GetAllAsync(bool paginated = false, int perPage = 10, int page = 1)
        {
            if (paginated)
            {
                var parameters = new Dictionary<string, object>
                {
                    {"p_per_page", perPage},
                    {"p_page", page}
                };

                return await ExecuteQueryAsync("GetAllCalendarEventsPaginatedProcedure", parameters);
            }

            return await ExecuteQueryAsync("GetAllCalendarEventsProcedure");
        }
    }
}
