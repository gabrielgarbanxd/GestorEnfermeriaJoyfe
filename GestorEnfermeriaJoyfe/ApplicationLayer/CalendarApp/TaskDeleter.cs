using GestorEnfermeriaJoyfe.Domain.Calendar;
using GestorEnfermeriaJoyfe.Domain.Calendar.ValueObjects;
using GestorEnfermeriaJoyfe.Infraestructure.CalendarPersistence;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.ApplicationLayer.CalendarApp
{
    public class TaskDeleter
    {
        private readonly ICalendarContract _calendarRepository;

        public TaskDeleter(ICalendarContract calendarRepository)
        {
            _calendarRepository = calendarRepository;
        }

        public async Task<bool> Run(int id)
        {
            return await _calendarRepository.DeleteAsync(id) > 0;
        }
    }
}
