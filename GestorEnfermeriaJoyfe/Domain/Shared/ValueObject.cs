using System;

namespace GestorEnfermeriaJoyfe.Domain.Shared
{
    public abstract class ValueObject<T> where T : IComparable
    {
        public T Value { get; private set; }

        protected ValueObject(T value)
        {
            Value = value;
            EnsureValueIsDefined(value);
        }

        private void EnsureValueIsDefined(T value)
        {
            if (value == null)
            {
                throw new ArgumentException("Value must be defined");
            }
        }

        public bool Equals(ValueObject<T> other)
        {
            return other.GetType() == GetType() && other.Value.CompareTo(Value) == 0;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
