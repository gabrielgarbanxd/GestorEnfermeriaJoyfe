using GestorEnfermeriaJoyfe.Domain.Shared;
using System;

namespace GestorEnfermeriaJoyfe.Domain.Calendar.ValueObjects
{
    public class CalendarId : NumberValueObject
    {
        private const int MinCalendarIdValue = 0;

        public CalendarId(int value) : base(value)
        {
            if (value < MinCalendarIdValue)
            {
                throw new ArgumentException("El identificador del evento en el calendario no puede ser menor o igual a 0.");
            }
        }
    }
}
