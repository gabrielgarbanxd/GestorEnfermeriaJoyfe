using GestorEnfermeriaJoyfe.Domain.ScheduledCiteRule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Adapters.ScheduledCiteRuleAdapters
{
    public class ScheduledCiteRuleResponse : ResponseBase<IEnumerable<ScheduledCiteRule>>
    {
        public ScheduledCiteRuleResponse(bool success, string? message, IEnumerable<ScheduledCiteRule>? data) : base(success, message, data)
        {
        }

        public ScheduledCiteRuleResponse(bool success, string? message) : base(success, message)
        {
        }

        public ScheduledCiteRuleResponse()
        {
        }

        public override ResponseBase<IEnumerable<ScheduledCiteRule>> Ok(string? message = null, IEnumerable<ScheduledCiteRule>? data = null)
        {
            return new ScheduledCiteRuleResponse(true, message, data);
        }

        public override ResponseBase<IEnumerable<ScheduledCiteRule>> Fail(string? message = null)
        {
            return new ScheduledCiteRuleResponse(false, message);
        }
    }
}
