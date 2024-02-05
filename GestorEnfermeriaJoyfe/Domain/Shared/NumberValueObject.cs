namespace GestorEnfermeriaJoyfe.Domain.Shared
{
    public abstract class NumberValueObject : ValueObject<int>
    {
        protected NumberValueObject(int value) : base(value){}

        public bool isBiggerThan(NumberValueObject number)
        {
            return this.Value > number.Value;
        }
    }
}
