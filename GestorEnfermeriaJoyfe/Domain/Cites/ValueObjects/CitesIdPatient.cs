using GestorEnfermeriaJoyfe.Domain.Shared;

namespace GestorEnfermeriaJoyfe.Domain.Cites.ValueObjects
{
    public sealed class CitesPatientId : IdValueObject
    {
        public CitesPatientId(int value) : base(value) { }
    }
}
