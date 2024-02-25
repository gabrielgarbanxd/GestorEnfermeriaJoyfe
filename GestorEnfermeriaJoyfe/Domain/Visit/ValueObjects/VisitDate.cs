using GestorEnfermeriaJoyfe.Domain.Shared;
using System;
namespace GestorEnfermeriaJoyfe.Domain.Visit.ValueObjects
{
    public sealed class VisitDate : DateTimeValueObject
    {
        public VisitDate(DateTime value) : base(value) { }

        public VisitDate() : base(DateTime.Now) { }
    }
}
