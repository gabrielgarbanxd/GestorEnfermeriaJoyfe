using GestorEnfermeriaJoyfe.Domain.Calendar;
using GestorEnfermeriaJoyfe.Domain.Calendar.ValueObjects;
using GestorEnfermeriaJoyfe.Infraestructure.CalendarPersistence;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.ApplicationLayer.CalendarApp
{
    public class TaskDeleter
    {
        private readonly ICalendarContract _calendarRepository;

        public TaskDeleter(ICalendarContract calendarRepository) => _calendarRepository = calendarRepository;

        public async Task<int> Run(int id) => await _calendarRepository.DeleteAsync(new CalendarId(id));
    }
}
