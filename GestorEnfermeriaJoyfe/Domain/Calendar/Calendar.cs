using GestorEnfermeriaJoyfe.Domain.Calendar.ValueObjects;
using System;

namespace GestorEnfermeriaJoyfe.Domain.Calendar
{
    public class Calendar
    {
        public CalendarId Id { get; private set; }
        public CalendarFecha Date { get; private set; }
        public CalendarTarea Task { get; private set; }

        public Calendar(CalendarId? id, CalendarFecha fecha, CalendarTarea tarea)
        {
            Id = id ?? new CalendarId(0);
            Date = fecha;
            Task = tarea;
        }

        public static Calendar Create(CalendarId? id, CalendarFecha fecha, CalendarTarea tarea)
        {
            return new Calendar(id, fecha, tarea);
        }

        public static Calendar FromPrimitives(int? id, DateTime fecha, string tarea)
        {
            return new Calendar(new CalendarId(id ?? 0), new CalendarFecha(fecha), new CalendarTarea(tarea));
        }
    }
}
