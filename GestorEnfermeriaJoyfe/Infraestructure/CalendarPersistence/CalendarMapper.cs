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
            int Id = reader.GetInt32(reader.GetOrdinal("id"));
            DateTime Date = reader.GetDateTime(reader.GetOrdinal("date"));
            string Task = reader.GetString(reader.GetOrdinal("task"));

            return Calendar.FromPrimitives(Id, Date, Task);
        }
    }
}
