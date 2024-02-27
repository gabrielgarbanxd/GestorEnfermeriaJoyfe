using GestorEnfermeriaJoyfe.Domain.Calendar;
using GestorEnfermeriaJoyfe.Domain.Calendar.ValueObjects;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.ApplicationLayer.CalendarApp
{
    internal class TaskFinder
    {
        private readonly ICalendarContract _calendarRepository;

        public TaskFinder(ICalendarContract calendarRepository) => _calendarRepository = calendarRepository;

        public async Task<Calendar> Run(int id) => await _calendarRepository.FindAsync(new CalendarId(id));
    }
}
