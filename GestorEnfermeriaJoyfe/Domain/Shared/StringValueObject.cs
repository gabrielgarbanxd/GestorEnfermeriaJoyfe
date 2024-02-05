namespace GestorEnfermeriaJoyfe.Domain.Shared
{
    public abstract class StringValueObject : ValueObject<string>
    {
        protected StringValueObject(string value) : base(value){}
    }
}
