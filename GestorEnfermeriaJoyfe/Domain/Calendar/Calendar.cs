using GestorEnfermeriaJoyfe.Domain.Calendar.ValueObjects;
using System;

namespace GestorEnfermeriaJoyfe.Domain.Calendar
{
    public class Calendar
    {
        public int Id { get; private set; }
        public CalendarFecha Fecha { get; private set; }
        public string Tarea { get; private set; }

        public Calendar(int id, CalendarFecha fecha, string tarea)
        {
            Id = id;
            Fecha = fecha;
            Tarea = tarea;
        }

        public static Calendar Create(int id, CalendarFecha fecha, string tarea)
        {
            return new Calendar(id, fecha, tarea);
        }

        public static Calendar FromPrimitives(int id, CalendarFecha fecha, string tarea)
        {
            return new Calendar(id, fecha, tarea);
        }
    }
}
