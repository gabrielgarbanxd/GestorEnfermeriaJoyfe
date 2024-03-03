using GestorEnfermeriaJoyfe.Domain.ScheduledCiteRule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.ApplicationLayer.ScheduledCiteRuleApp
{
    public class ScheduledCiteRulesByPatientIdSearcher
    {
        private readonly IScheduledCiteRuleContract _scheduledCiteRuleRepository;

        public ScheduledCiteRulesByPatientIdSearcher(IScheduledCiteRuleContract scheduledCiteRuleRepository) => _scheduledCiteRuleRepository = scheduledCiteRuleRepository;

        public async Task<IEnumerable<ScheduledCiteRule>> Run(int patientId, bool paginated = false, int perPage = 10, int page = 1)
        {
            if (paginated)
            {
                if (perPage <= 0 || page <= 0)
                {
                    throw new ArgumentException("El número de página y el número de elementos por página deben ser mayores a 0.");
                }

                return await _scheduledCiteRuleRepository.SearchByPatientIdAsync(new (patientId), paginated, perPage, page);
            }

            return await _scheduledCiteRuleRepository.SearchByPatientIdAsync(new(patientId));
        }
    }
}
