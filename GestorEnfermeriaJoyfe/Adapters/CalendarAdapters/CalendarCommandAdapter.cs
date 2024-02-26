using GestorEnfermeriaJoyfe.ApplicationLayer.CalendarApp;
using GestorEnfermeriaJoyfe.Domain.Calendar;
using GestorEnfermeriaJoyfe.Domain.Calendar.ValueObjects;
using GestorEnfermeriaJoyfe.Infraestructure.CalendarPersistence;
using System;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Adapters.CalendarAdapters
{
    public class CalendarCommandAdapter
    {
        private readonly ICalendarContract calendarRepository;

        public CalendarCommandAdapter(ICalendarContract calendarRepository)
        {
            this.calendarRepository = calendarRepository;
        }

        public async Task<Response<int>> CreateCalendarEntry(Calendar calendarEntry)
        {
            try
            {
                TaskCreator calendarCreator = new(calendarRepository);
                int newCalendarEntryId = await calendarCreator.Run(calendarEntry);

                return Response<int>.Ok("Entrada de calendario registrada", newCalendarEntryId);
            }
            catch (Exception e)
            {
                return Response<int>.Fail(e.Message);
            }
        }

        public async Task<Response<bool>> DeleteCalendarEntry(int id)
        {
            try
            {
                TaskDeleter calendarDeleter = new TaskDeleter(calendarRepository);
                bool deleted = await calendarDeleter.Run(id);

                return deleted ? Response<bool>.Ok("Entrada de calendario eliminada") : Response<bool>.Fail("Entrada de calendario no eliminada");
            }
            catch (Exception e)
            {
                return Response<bool>.Fail(e.Message);
            }
        }

        public async Task<Response<bool>> UpdateCalendarEntry(Calendar calendarEntry)
        {
            try
            {
                TaskUpdater calendarUpdater = new TaskUpdater(calendarRepository);
                bool updated = await calendarUpdater.Run(calendarEntry);

                return updated ? Response<bool>.Ok("Entrada de calendario actualizada") : Response<bool>.Fail("Entrada de calendario no actualizada");
            }
            catch (Exception e)
            {
                return Response<bool>.Fail(e.Message);
            }
        }
    }
}
