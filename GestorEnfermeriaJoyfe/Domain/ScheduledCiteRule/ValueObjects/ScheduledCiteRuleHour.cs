using GestorEnfermeriaJoyfe.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Domain.ScheduledCiteRule.ValueObjects
{
    public class ScheduledCiteRuleHour : ValueObject<TimeSpan>
    {
        public ScheduledCiteRuleHour(TimeSpan value) : base(value) {}
    }
}
