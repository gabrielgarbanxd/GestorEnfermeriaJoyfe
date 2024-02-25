using GestorEnfermeriaJoyfe.Domain.Shared;

namespace GestorEnfermeriaJoyfe.Domain.Visit.ValueObjects
{
    public sealed class VisitIsComunicated : BoolValueObject
    {
        public VisitIsComunicated(bool value) : base(value) { }

        public static VisitIsComunicated FromInt(int value) => new(value == 1);
        public static VisitIsComunicated FromString(string value) => new(value == "Sí");
    }
}
