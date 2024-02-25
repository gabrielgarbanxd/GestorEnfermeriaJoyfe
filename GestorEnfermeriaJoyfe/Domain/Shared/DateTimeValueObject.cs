using GestorEnfermeriaJoyfe.Domain.Shared;
using System;

namespace GestorEnfermeriaJoyfe.Domain.Shared
{
    public abstract class DateTimeValueObject : ValueObject<DateTime>
    {
        protected DateTimeValueObject(DateTime value) : base(value) { }

        public override string ToString()
        { 
            return Value.ToString("dd/MM/yyyy");
        }


    }
}
