using GestorEnfermeriaJoyfe.Domain.Calendar;
using GestorEnfermeriaJoyfe.Infraestructure.CalendarPersistence;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Adapters.CalendarAdapters
{
    public class CalendarController
    {
        private readonly CalendarMapper calendarMapper;
        private readonly MySqlCalendarRepository calendarRepository;

        private readonly CalendarQueryAdapter calendarQueryAdapter;
        private readonly CalendarCommandAdapter calendarCommandAdapter;

        public CalendarController()
        {
            calendarMapper = new CalendarMapper();
            calendarRepository = new MySqlCalendarRepository(calendarMapper);

            calendarQueryAdapter = new CalendarQueryAdapter(calendarRepository);
            calendarCommandAdapter = new CalendarCommandAdapter(calendarRepository);
        }

        public async Task<Response<List<Calendar>>> GetAll() => await calendarQueryAdapter.GetAllCalendarEntries();

        public async Task<Response<int>> Register(Calendar calendarEntry) => await calendarCommandAdapter.CreateCalendarEntry(calendarEntry);

        public async Task<Response<bool>> Update(Calendar calendarEntry) => await calendarCommandAdapter.UpdateCalendarEntry(calendarEntry);

        public async Task<Response<bool>> Delete(int id) => await calendarCommandAdapter.DeleteCalendarEntry(id);
    }
}
