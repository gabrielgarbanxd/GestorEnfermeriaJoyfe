using GestorEnfermeriaJoyfe.Domain.Shared;
using System;

namespace GestorEnfermeriaJoyfe.Domain.Cites.ValueObjects
{
    public sealed class CitesDate : DateTimeValueObject
    {
        public CitesDate(DateTime value) : base(value) { }

        public CitesDate() : base(DateTime.Now) { }
    }
}
