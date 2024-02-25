using GestorEnfermeriaJoyfe.Domain.Shared;

namespace GestorEnfermeriaJoyfe.Domain.Visit.ValueObjects
{
    public sealed class VisitIsDerived : BoolValueObject
    {
        public VisitIsDerived(bool value) : base(value) { }

        public static VisitIsDerived FromInt(int value) => new(value == 1);
        public static VisitIsDerived FromString(string value) => new(value == "Sí");
    }
}
