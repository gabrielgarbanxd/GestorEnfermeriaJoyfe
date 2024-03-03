using GestorEnfermeriaJoyfe.Domain.ScheduledCiteRule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.ApplicationLayer.ScheduledCiteRuleApp
{
    public class ScheduledCiteRuleDeleter
    {
        private readonly IScheduledCiteRuleContract _scheduledCiteRuleRepository;

        public ScheduledCiteRuleDeleter(IScheduledCiteRuleContract scheduledCiteRuleRepository) => _scheduledCiteRuleRepository = scheduledCiteRuleRepository;

        public async Task<int> Run(int id) => await _scheduledCiteRuleRepository.DeleteAsync(id);
    }
}
