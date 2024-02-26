using GestorEnfermeriaJoyfe.Domain.Calendar;
using GestorEnfermeriaJoyfe.Infraestructure.CalendarPersistence;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Adapters.CalendarAdapters
{
    public class CalendarQueryAdapter
    {
        private readonly ICalendarContract calendarRepository;

        public CalendarQueryAdapter(ICalendarContract calendarRepository)
        {
            this.calendarRepository = calendarRepository;
        }

        public async Task<Response<Calendar>> FindCalendarEntry(int id)
        {
            try
            {
                var calendarEntry = await calendarRepository.FindAsync(id);

                return calendarEntry != null ? Response<Calendar>.Ok("Entrada de calendario encontrada", calendarEntry) : Response<Calendar>.Fail("Entrada de calendario no encontrada");
            }
            catch (Exception e)
            {
                return Response<Calendar>.Fail(e.Message);
            }
        }

        public async Task<Response<List<Calendar>>> GetAllCalendarEntries()
        {
            try
            {
                var calendarEntries = await calendarRepository.GetAllAsync();

                return calendarEntries.Count > 0 ? Response<List<Calendar>>.Ok("Entradas de calendario encontradas", calendarEntries) : Response<List<Calendar>>.Fail("No se encontraron entradas de calendario");
            }
            catch (Exception e)
            {
                return Response<List<Calendar>>.Fail(e.Message);
            }
        }
    }
}
