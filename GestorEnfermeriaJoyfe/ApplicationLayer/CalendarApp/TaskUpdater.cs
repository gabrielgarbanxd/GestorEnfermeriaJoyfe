using GestorEnfermeriaJoyfe.Domain.Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.ApplicationLayer.CalendarApp
{
    public class TaskUpdater
    {
        private readonly ICalendarContract _calendarRepository;

        public TaskUpdater(ICalendarContract calendarRepository) => _calendarRepository = calendarRepository;

        public async Task<int> Run(Calendar calendarEntry) => await _calendarRepository.UpdateAsync(calendarEntry);
    }
}
