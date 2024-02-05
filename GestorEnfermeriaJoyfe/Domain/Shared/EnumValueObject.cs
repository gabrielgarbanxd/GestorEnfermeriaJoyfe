using System;
using System.Collections.Generic;

namespace GestorEnfermeriaJoyfe.Domain.Shared
{
    public abstract class EnumValueObject<T>
    {
        public T Value { get; private set; }
        public List<T> ValidValues { get; private set; }

        protected EnumValueObject(T value, List<T> validValues)
        {
            ValidValues = validValues;
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
