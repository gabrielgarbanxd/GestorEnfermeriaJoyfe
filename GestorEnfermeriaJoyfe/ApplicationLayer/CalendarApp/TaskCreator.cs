using GestorEnfermeriaJoyfe.Domain.Calendar;
using GestorEnfermeriaJoyfe.Infraestructure.CalendarPersistence;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.ApplicationLayer.CalendarApp
{
    public class TaskCreator
    {
        private readonly ICalendarContract _calendarRepository;

        public TaskCreator(ICalendarContract calendarRepository) => _calendarRepository = calendarRepository;

        public async Task<int> Run(Calendar calendarEntry) => await _calendarRepository.AddAsync(calendarEntry);
    }
}
