using GestorEnfermeriaJoyfe.Domain.Calendar;
using System.Collections.Generic;

namespace GestorEnfermeriaJoyfe.Adapters.CalendarAdapters
{
    public class CalendarResponse : ResponseBase<IEnumerable<Calendar>>
    {
        public CalendarResponse(bool success, string? message, IEnumerable<Calendar>? data) : base(success, message, data)
        {
        }

        public CalendarResponse(bool success, string? message) : base(success, message)
        {
        }

        public CalendarResponse()
        {
        }

        public override ResponseBase<IEnumerable<Calendar>> Ok(string? message = null, IEnumerable<Calendar>? data = null)
        {
            return new CalendarResponse(true, message, data);
        }

        public override ResponseBase<IEnumerable<Calendar>> Fail(string? message = null)
        {
            return new CalendarResponse(false, message);
        }
    }
}
