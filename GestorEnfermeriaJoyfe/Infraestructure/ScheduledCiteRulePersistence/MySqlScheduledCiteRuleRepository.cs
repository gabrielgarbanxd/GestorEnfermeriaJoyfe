using GestorEnfermeriaJoyfe.Domain.Patient.ValueObjects;
using GestorEnfermeriaJoyfe.Domain.ScheduledCiteRule;
using GestorEnfermeriaJoyfe.Domain.ScheduledCiteRule.ValueObjects;
using GestorEnfermeriaJoyfe.Infraestructure.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Infraestructure.ScheduledCiteRulePersistence
{
    class MySqlScheduledCiteRuleRepository : MySqlRepositoryBase<ScheduledCiteRule>, IScheduledCiteRuleContract
    {
        public MySqlScheduledCiteRuleRepository(ScheduledCiteRuleMapper mapper): base(mapper)
        {
        }

        public MySqlScheduledCiteRuleRepository() { }

        public async Task<IEnumerable<ScheduledCiteRule>> GetAllAsync(bool paginated = false, int perPage = 10, int page = 1)
        {
            if (paginated)
            {
                var parameters = new Dictionary<string, object>
                {
                    { "p_per_page", perPage },
                    { "p_page", page }
                };

                return await ExecuteQueryAsync("GetAllScheduledCiteRulesPaginatedProcedure", parameters);
            }

            return await ExecuteQueryAsync("GetAllScheduledCiteRulesProcedure");
        }

        public async Task<ScheduledCiteRule> FindAsync(ScheduledCiteRuleId scheduledCiteRuleId)
        {
            var result = await ExecuteQueryAsync("GetScheduledCiteRuleByIdProcedure", new Dictionary<string, object> { { "p_id", scheduledCiteRuleId.Value } });

            if (!result.Any())
            {
                throw new Exception("No se ha encontrado la regla de cita programada.");
            }

            return result.First();
        }


        public async Task<int> CreateAsync(ScheduledCiteRule scheduledCiteRule)
        {
            var parameters = new Dictionary<string, object>
            {
                { "p_name", scheduledCiteRule.Name.Value },
                { "p_hour", scheduledCiteRule.Hour.Value },
                { "p_start_date", scheduledCiteRule.StartDate.Value },
                { "p_end_date", scheduledCiteRule.EndDate.Value },
                { "p_lunes", scheduledCiteRule.Lunes.ToInt() },
                { "p_martes", scheduledCiteRule.Martes.ToInt() },
                { "p_miercoles", scheduledCiteRule.Miercoles.ToInt() },
                { "p_jueves", scheduledCiteRule.Jueves.ToInt() },
                { "p_viernes", scheduledCiteRule.Viernes.ToInt() },
                { "p_patient_id", scheduledCiteRule.PatientId.Value }
            };

            var result = await ExecuteNonQueryAsync("CreateScheduledCiteRuleProcedure", parameters);

            return result <= 0 ? throw new Exception("No se ha podido crear la regla de cita programada.") : result;
        }

        public async Task<int> UpdateAsync(ScheduledCiteRule scheduledCiteRule)
        {
            var parameters = new Dictionary<string, object>
            {
                { "p_id", scheduledCiteRule.Id.Value },
                { "p_name", scheduledCiteRule.Name.Value },
                { "p_hour", scheduledCiteRule.Hour.Value },
                { "p_start_date", scheduledCiteRule.StartDate.Value },
                { "p_end_date", scheduledCiteRule.EndDate.Value },
                { "p_lunes", scheduledCiteRule.Lunes.ToInt() },
                { "p_martes", scheduledCiteRule.Martes.ToInt() },
                { "p_miercoles", scheduledCiteRule.Miercoles.ToInt() },
                { "p_jueves", scheduledCiteRule.Jueves.ToInt() },
                { "p_viernes", scheduledCiteRule.Viernes.ToInt() },
                { "p_patient_id", scheduledCiteRule.PatientId.Value }
            };

            var result = await ExecuteNonQueryAsync("UpdateScheduledCiteRuleProcedure", parameters);

            return result <= 0 ? throw new Exception("No se ha podido actualizar la regla de cita programada.") : result;
        }

        public async Task<int> DeleteAsync(ScheduledCiteRuleId scheduledCiteRuleId)
        {
            var result = await ExecuteNonQueryAsync("DeleteScheduledCiteRuleProcedure", new Dictionary<string, object> { { "p_id", scheduledCiteRuleId.Value } });

            return result <= 0 ? throw new Exception("No se ha podido eliminar la regla de cita programada.") : result;
        }

        public async Task<IEnumerable<ScheduledCiteRule>> SearchByPatientIdAsync(PatientId patientId, bool paginated = false, int perPage = 10, int page = 1)
        {
            var parameters = new Dictionary<string, object>
            {
                { "p_patient_id", patientId.Value }
            };

            if (paginated)
            {
                parameters.Add("p_per_page", perPage);
                parameters.Add("p_page", page);

                return await ExecuteQueryAsync("SearchScheduledCiteRuleByPatientIdPaginatedProcedure", parameters);
            }

            return await ExecuteQueryAsync("SearchScheduledCiteRuleByPatientIdProcedure", parameters);
        }
    }
}
