using GestorEnfermeriaJoyfe.Domain.Shared;
using System;

namespace GestorEnfermeriaJoyfe.Domain.Calendar.ValueObjects
{
    public class CalendarFecha : DateTimeValueObject
    {
        public CalendarFecha(DateTime value) : base(value)
        {
            // Verificar que la fecha no sea en el pasado
            if (value <= DateTime.Now)
            {
                throw new ArgumentException("La fecha del evento en el calendario no puede ser en el pasado.");
            }

        }
    }
}
