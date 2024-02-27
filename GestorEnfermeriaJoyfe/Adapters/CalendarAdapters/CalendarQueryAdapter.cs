using GestorEnfermeriaJoyfe.ApplicationLayer.CalendarApp;
using GestorEnfermeriaJoyfe.Domain.Calendar;
using GestorEnfermeriaJoyfe.Infraestructure.CalendarPersistence;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Adapters.CalendarAdapters
{
    public class CalendarQueryAdapter : QueryAdapterBase<CalendarResponse, IEnumerable<Calendar>>
    {
        private readonly ICalendarContract _calendarRepository;

        public CalendarQueryAdapter(ICalendarContract calendarRepository, CalendarResponse response) : base(response)
        {
            _calendarRepository = calendarRepository;
        }

        public async Task<CalendarResponse> FindCalendarEntry(int id)
        {
            return await RunQuery(async () =>
            {
                return new List<Calendar> { await new TaskFinder(_calendarRepository).Run(id) };
            });
        }

        public async Task<CalendarResponse> GetAllCalendarEntries()
        {
            return await RunQuery(async () =>
            {
                return await new TaskLister(_calendarRepository).Run();
            });
        }

        public async Task<CalendarResponse> GetAllCalendarEntriesPaginated(int perPage, int page)
        {
            return await RunQuery(async () =>
            {
                return await new TaskLister(_calendarRepository).Run(true, perPage, page);
            });
        }
    }
}
