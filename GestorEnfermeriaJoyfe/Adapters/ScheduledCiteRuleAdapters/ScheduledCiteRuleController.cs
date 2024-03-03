using GestorEnfermeriaJoyfe.Domain.ScheduledCiteRule;
using GestorEnfermeriaJoyfe.Infraestructure.ScheduledCiteRulePersistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Adapters.ScheduledCiteRuleAdapters
{
    public class ScheduledCiteRuleController
    {
        private readonly ScheduledCiteRuleMapper scheduledCiteRuleMapper;
        private readonly MySqlScheduledCiteRuleRepository scheduledCiteRuleRepository;

        private readonly ScheduledCiteRuleQueryAdapter scheduledCiteRuleQueryAdapter;
        private readonly ScheduledCiteRuleCommandAdapter scheduledCiteRuleCommandAdapter;

        public ScheduledCiteRuleController()
        {
            scheduledCiteRuleMapper = new();
            scheduledCiteRuleRepository = new(scheduledCiteRuleMapper);

            scheduledCiteRuleQueryAdapter = new ScheduledCiteRuleQueryAdapter(scheduledCiteRuleRepository, new());
            scheduledCiteRuleCommandAdapter = new ScheduledCiteRuleCommandAdapter(scheduledCiteRuleRepository);
        }

        // ================== QUERYS ==================

        public async Task<ScheduledCiteRuleResponse> GetAll() => await scheduledCiteRuleQueryAdapter.GetAllScheduledCiteRules();
        public async Task<ScheduledCiteRuleResponse> GetAllPaginated(int perPage, int page) => await scheduledCiteRuleQueryAdapter.GetAllScheduledCiteRulesPaginated(perPage, page);
        public async Task<ScheduledCiteRuleResponse> Get(int id) => await scheduledCiteRuleQueryAdapter.FindScheduledCiteRule(id);
        public async Task<ScheduledCiteRuleResponse> SearchByPatientId(int patientId) => await scheduledCiteRuleQueryAdapter.SearchByPatientId(patientId);


        // ================== COMMANDS ==================

        public async Task<CommandResponse> Create(ScheduledCiteRule scheduledCiteRule) => await scheduledCiteRuleCommandAdapter.CreateScheduledCiteRule(scheduledCiteRule);
        public async Task<CommandResponse> Update(ScheduledCiteRule scheduledCiteRule) => await scheduledCiteRuleCommandAdapter.UpdateScheduledCiteRule(scheduledCiteRule);
        public async Task<CommandResponse> Delete(int id) => await scheduledCiteRuleCommandAdapter.DeleteScheduledCiteRule(id);

    }
}
