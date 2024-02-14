using System;
using System.Data;
using GestorEnfermeriaJoyfe.Domain.Calendar;
using GestorEnfermeriaJoyfe.Domain.Calendar.ValueObjects;

namespace GestorEnfermeriaJoyfe.Infraestructure.CalendarPersistence
{
    public class CalendarMapper : IObjectMapper
    {
        public T Map<T>(IDataReader reader)
        {
            if (typeof(T) == typeof(Calendar))
            {
                int id = reader.GetInt32(reader.GetOrdinal("id"));
                DateTime fecha = reader.GetDateTime(reader.GetOrdinal("fecha"));
                string tarea = reader.GetString(reader.GetOrdinal("tarea"));

                Calendar calendar = new Calendar(
                   id,
                   new CalendarFecha(fecha),
                   tarea
               );

                return (T)(object)calendar;
            }

            throw new ArgumentException($"No mapper available for type {typeof(T)}");
        }
    }
}
