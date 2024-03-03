using GestorEnfermeriaJoyfe.Domain.ScheduledCiteRule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.ApplicationLayer.ScheduledCiteRuleApp
{
    public class ScheduledCiteRuleUpdater
    {
        private readonly IScheduledCiteRuleContract _scheduledCiteRuleRepository;


        public ScheduledCiteRuleUpdater(IScheduledCiteRuleContract scheduledCiteRuleRepository) => _scheduledCiteRuleRepository = scheduledCiteRuleRepository;

        public async Task<int> Run(ScheduledCiteRule scheduledCiteRule) => await _scheduledCiteRuleRepository.UpdateAsync(scheduledCiteRule);
    }
}
