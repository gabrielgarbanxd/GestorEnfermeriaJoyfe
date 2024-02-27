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

            calendarQueryAdapter = new CalendarQueryAdapter(calendarRepository, new());
            calendarCommandAdapter = new CalendarCommandAdapter(calendarRepository);
        }

        // ================== QUERYS ==================

        //public async Task<Response<List<Calendar>>> GetAll() => await calendarQueryAdapter.GetAllCalendarEntries();

        public async Task<CalendarResponse> GetAll() => await calendarQueryAdapter.GetAllCalendarEntries();
        public async Task<CalendarResponse> GetAllPaginated(int perPage, int page) => await calendarQueryAdapter.GetAllCalendarEntriesPaginated(perPage, page);
        public async Task<CalendarResponse> Get(int id) => await calendarQueryAdapter.FindCalendarEntry(id);


        // ================== COMMANDS ==================

        public async Task<CommandResponse> Register(Calendar calendarEntry) => await calendarCommandAdapter.CreateCalendarEntry(calendarEntry);

        public async Task<CommandResponse> Update(Calendar calendarEntry) => await calendarCommandAdapter.UpdateCalendarEntry(calendarEntry);

        public async Task<CommandResponse> Delete(int id) => await calendarCommandAdapter.DeleteCalendarEntry(id);
    }
}
