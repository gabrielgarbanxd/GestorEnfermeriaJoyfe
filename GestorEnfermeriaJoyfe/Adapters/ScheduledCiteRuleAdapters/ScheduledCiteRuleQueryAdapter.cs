using GestorEnfermeriaJoyfe.ApplicationLayer.ScheduledCiteRuleApp;
using GestorEnfermeriaJoyfe.Domain.ScheduledCiteRule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Adapters.ScheduledCiteRuleAdapters
{
    public class ScheduledCiteRuleQueryAdapter : QueryAdapterBase<ScheduledCiteRuleResponse, IEnumerable<ScheduledCiteRule>>
    {
        private readonly IScheduledCiteRuleContract scheduledCiteRuleRepository;

        public ScheduledCiteRuleQueryAdapter(IScheduledCiteRuleContract scheduledCiteRuleRepository, ScheduledCiteRuleResponse response) : base(response)
        {
            this.scheduledCiteRuleRepository = scheduledCiteRuleRepository;
        }

        public async Task<ScheduledCiteRuleResponse> FindScheduledCiteRule(int id)
        {
            return await RunQuery(async () =>
            {
                return new List<ScheduledCiteRule> { await new ScheduledCiteRuleFinder(scheduledCiteRuleRepository).Run(id) };
            });
        }

        public async Task<ScheduledCiteRuleResponse> GetAllScheduledCiteRules()
        {
            return await RunQuery(async () =>
            {
                return await new ScheduledCiteRuleLister(scheduledCiteRuleRepository).Run();
            });
        }

        public async Task<ScheduledCiteRuleResponse> GetAllScheduledCiteRulesPaginated(int perPage, int page)
        {
            return await RunQuery(async () =>
            {
                return await new ScheduledCiteRuleLister(scheduledCiteRuleRepository).Run(true, perPage, page);
            });
        }

        public async Task<ScheduledCiteRuleResponse> SearchByPatientId(int patientId)
        {
            return await RunQuery(async () =>
            {
                return await new ScheduledCiteRulesByPatientIdSearcher(scheduledCiteRuleRepository).Run(patientId);
            });
        }

        public async Task<ScheduledCiteRuleResponse> SearchByPatientIdPaginated(int patientId, int perPage, int page)
        {
            return await RunQuery(async () =>
            {
                return await new ScheduledCiteRulesByPatientIdSearcher(scheduledCiteRuleRepository).Run(patientId, true, perPage, page);
            });
        }
    }
}
