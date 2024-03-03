using GestorEnfermeriaJoyfe.Domain.Shared;

namespace GestorEnfermeriaJoyfe.Domain.ScheduledCiteRule.ValueObjects
{
    public class ScheduledCiteRuleDay : BoolValueObject
    {
        public ScheduledCiteRuleDay(bool value) : base(value) { }

        public static ScheduledCiteRuleDay FromInt(int value) => new(value == 1);

        public string ToCheckString => Value ? "✓" : "✗";

    }
}
