using GestorEnfermeriaJoyfe.Domain.Shared;
using System;

namespace GestorEnfermeriaJoyfe.Domain.Shared
{
    public abstract class DateTimeValueObject : ValueObject<DateTime>
    {
        protected DateTimeValueObject(DateTime value) : base(value) { }

        public string ToDate => Value.ToString("dd/MM/yyyy");

        public string ToLongString => Value.ToString("G");

        public string ToTime => Value.ToString("HH:mm");

        public string ToDayOfMonth => Value.ToString("M");

    }
}
