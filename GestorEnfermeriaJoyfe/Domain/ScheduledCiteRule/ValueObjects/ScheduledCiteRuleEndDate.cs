using GestorEnfermeriaJoyfe.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Domain.ScheduledCiteRule.ValueObjects
{
    public class ScheduledCiteRuleEndDate : DateTimeValueObject
    {
        public ScheduledCiteRuleEndDate(DateTime value) : base(value) { }
    }
}
