using System;
using System.Data;
using GestorEnfermeriaJoyfe.Domain.Calendar;
using GestorEnfermeriaJoyfe.Domain.Calendar.ValueObjects;
using GestorEnfermeriaJoyfe.Infraestructure.Shared;

namespace GestorEnfermeriaJoyfe.Infraestructure.CalendarPersistence
{
    public class CalendarMapper : IObjectMapper<Calendar>
    {
        public Calendar Map(IDataReader reader)
        {
            int id = reader.GetInt32(reader.GetOrdinal("id"));
            DateTime fecha = reader.GetDateTime(reader.GetOrdinal("fecha"));
            string tarea = reader.GetString(reader.GetOrdinal("tarea"));

            return Calendar.FromPrimitives(id, fecha, tarea);
        }
    }
}
