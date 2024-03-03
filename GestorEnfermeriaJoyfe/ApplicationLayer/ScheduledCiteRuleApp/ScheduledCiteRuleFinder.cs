using GestorEnfermeriaJoyfe.Domain.ScheduledCiteRule;
using GestorEnfermeriaJoyfe.Domain.ScheduledCiteRule.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.ApplicationLayer.ScheduledCiteRuleApp
{
    public class ScheduledCiteRuleFinder
    {
        private readonly IScheduledCiteRuleContract _scheduledCiteRuleRepository;

        public ScheduledCiteRuleFinder(IScheduledCiteRuleContract scheduledCiteRuleRepository) => _scheduledCiteRuleRepository = scheduledCiteRuleRepository;

        public async Task<ScheduledCiteRule> Run(int id) => await _scheduledCiteRuleRepository.FindAsync(new ScheduledCiteRuleId(id));

    }
}
