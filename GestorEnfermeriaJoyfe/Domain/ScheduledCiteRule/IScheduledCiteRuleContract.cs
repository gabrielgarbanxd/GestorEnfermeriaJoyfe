using GestorEnfermeriaJoyfe.Domain.Patient.ValueObjects;
using GestorEnfermeriaJoyfe.Domain.ScheduledCiteRule.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Domain.ScheduledCiteRule
{
    public interface IScheduledCiteRuleContract
    {
        Task<IEnumerable<ScheduledCiteRule>> GetAllAsync(bool paginated = false, int perPage = 10, int page = 1);
        Task<ScheduledCiteRule> FindAsync(ScheduledCiteRuleId scheduledCiteRuleId);
        Task<int> CreateAsync(ScheduledCiteRule scheduledCiteRule);
        Task<int> UpdateAsync(ScheduledCiteRule scheduledCiteRule);
        Task<int> DeleteAsync(ScheduledCiteRuleId scheduledCiteRuleId);
        Task<IEnumerable<ScheduledCiteRule>> SearchByPatientIdAsync(PatientId patientId, bool paginated = false, int perPage = 10, int page = 1);
    }
}
