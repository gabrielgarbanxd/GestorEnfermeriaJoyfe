using GestorEnfermeriaJoyfe.Domain.Calendar;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.ApplicationLayer.CalendarApp
{
    internal class TaskLister
    {
        private readonly ICalendarContract _calendarRepository;

        public TaskLister(ICalendarContract calendarRepository) => _calendarRepository = calendarRepository;

        public async Task<IEnumerable<Calendar>> Run(bool paginated = false, int perPage = 10, int page = 1)
        {
            if (paginated)
            {
                if (perPage <= 0 || page <= 0)
                {
                    throw new System.ArgumentException("El número de página y el número de elementos por página deben ser mayores a 0.");
                }

                return await _calendarRepository.GetAllAsync(paginated, perPage, page);
            }

            return await _calendarRepository.GetAllAsync();
        }
    }
}
