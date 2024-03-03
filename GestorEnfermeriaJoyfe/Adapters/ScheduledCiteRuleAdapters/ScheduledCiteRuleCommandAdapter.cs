using GestorEnfermeriaJoyfe.ApplicationLayer.ScheduledCiteRuleApp;
using GestorEnfermeriaJoyfe.Domain.ScheduledCiteRule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Adapters.ScheduledCiteRuleAdapters
{
    public class ScheduledCiteRuleCommandAdapter :CommandAdapterBase
    {
        private readonly IScheduledCiteRuleContract scheduledCiteRuleRepository;

        public ScheduledCiteRuleCommandAdapter(IScheduledCiteRuleContract scheduledCiteRuleRepository) => this.scheduledCiteRuleRepository = scheduledCiteRuleRepository;

        public async Task<CommandResponse> CreateScheduledCiteRule(ScheduledCiteRule scheduledCiteRule)
        {
            return await RunCommand(async () =>
            {
                return await new ScheduledCiteRuleCreator(scheduledCiteRuleRepository).Run(scheduledCiteRule);
            });
        }

        public async Task<CommandResponse> UpdateScheduledCiteRule(ScheduledCiteRule scheduledCiteRule)
        {
            return await RunCommand(async () =>
            {
                return await new ScheduledCiteRuleUpdater(scheduledCiteRuleRepository).Run(scheduledCiteRule);
            });
        }

        public async Task<CommandResponse> DeleteScheduledCiteRule(int id)
        {
            return await RunCommand(async () =>
            {
                return await new ScheduledCiteRuleDeleter(scheduledCiteRuleRepository).Run(id);
            });
        }
    }
}
