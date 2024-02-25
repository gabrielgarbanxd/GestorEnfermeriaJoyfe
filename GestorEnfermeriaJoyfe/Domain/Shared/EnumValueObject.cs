using System;
using System.Collections.Generic;
using System.Linq;

namespace GestorEnfermeriaJoyfe.Domain.Shared
{
    public abstract class EnumValueObject<T>
    {
        public T Value { get; private set; }
        public abstract HashSet<T> ValidValues { get; }

        protected EnumValueObject(T value)
        {

            if (!CheckValueIsValid(value))
            {
                throw new ArgumentException("Invalid value");
            }

            Value = value;
        }

        private bool CheckValueIsValid(T value)
        {
            return ValidValues.Contains(value);
        }

        public bool Equals(EnumValueObject<T> other)
        {
            if (other == null)
            {
                return false;
            }

            return EqualityComparer<T>.Default.Equals(Value, other.Value);
        }
    }
}
