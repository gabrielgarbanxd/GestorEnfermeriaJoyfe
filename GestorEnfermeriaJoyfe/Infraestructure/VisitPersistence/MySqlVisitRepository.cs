using GestorEnfermeriaJoyfe.Domain.Patient.ValueObjects;
using GestorEnfermeriaJoyfe.Domain.Visit;
using GestorEnfermeriaJoyfe.Domain.Visit.ValueObjects;
using GestorEnfermeriaJoyfe.Infraestructure.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Infraestructure.VisitPersistence
{
    public class MySqlVisitRepository : MySqlRepositoryBase<Visit>, IVisitContract
    {
        public MySqlVisitRepository(VisitMapper mapper) : base(mapper)
        {
        }

        public MySqlVisitRepository()
        {
        }

        public  async Task<IEnumerable<Visit>> GetAllAsync(bool paginated = false, int perPage = 10, int page = 1)
        {
            if (paginated)
            {
                var parameters = new Dictionary<string, object>
                {
                    {"p_per_page", perPage},
                    {"p_page", page}
                };

                return await ExecuteQueryAsync("GetAllVisitsPaginatedProcedure", parameters);
            }

            return await ExecuteQueryAsync("GetAllVisitsProcedure");
        }

        public async Task<Visit> FindAsync(VisitId visitId)
        {
            var result = await ExecuteQueryAsync("GetVisitByIdProcedure", new Dictionary<string, object> { { "p_id", visitId.Value } });

            if (!result.Any())
            {
                throw new System.Exception("No se ha encontrado la visita.");
            }

            return result.First();
        }

        public async Task<int> CreateAsync(Visit visit)
        {
            var parameters = new Dictionary<string, object>
            {
                {"p_type", visit.Type.Value},
                {"p_classification", visit.Classification.Value},
                {"p_description", visit.Description?.Value ?? ""},
                {"p_is_comunicated",visit.IsComunicated.ToInt()},
                {"p_is_derived",visit.IsDerived.ToInt()},
                {"p_trauma_type",visit.TraumaType?.Value ?? ""},
                {"p_place",visit.Place?.Value ?? ""},
                {"p_date", visit.Date.Value},
                {"p_patient_id",visit.PatientId.Value}
            };

            var result = await ExecuteNonQueryAsync("CreateVisitProcedure", parameters);

            return result <= 0 ? throw new Exception("No se ha podido crear la visita.") : result;
        }

        public async Task<int> UpdateAsync(Visit visit)
        {
            var parameters = new Dictionary<string, object>
            {
                {"p_id", visit.Id.Value},
                {"p_type", visit.Type.Value},
                {"p_classification", visit.Classification.Value},
                {"p_description", visit.Description?.Value ?? ""},
                {"p_is_comunicated",visit.IsComunicated.ToInt()},
                {"p_is_derived",visit.IsDerived.ToInt()},
                {"p_trauma_type",visit.TraumaType?.Value ?? ""},
                {"p_place",visit.Place?.Value ?? ""},
                {"p_date", visit.Date.Value},
                {"p_patient_id",visit.PatientId.Value}
            };
            var result = await ExecuteNonQueryAsync("UpdateVisitProcedure", parameters);

            return result <= 0 ? throw new Exception("No se ha podido actualizar la visita.") : result;
        }

        public async Task<int> DeleteAsync(VisitId visitId)
        {
            var result = await ExecuteNonQueryAsync("DeleteVisitProcedure", new Dictionary<string, object> { { "p_id", visitId.Value } });

            return result <= 0 ? throw new Exception("No se ha podido eliminar la visita.") : result;
        }

        public async Task<IEnumerable<Visit>> SearchByPatientIdAsync(PatientId patientId, bool paginated = false, int perPage = 10, int page = 1)
        {
            var parameters = new Dictionary<string, object> { { "p_patient_id", patientId.Value } };

            if (paginated)
            {
                parameters.Add("p_per_page", perPage);
                parameters.Add("p_page", page);

                return await ExecuteQueryAsync("GetVisitsByPatientIdPaginatedProcedure", parameters);
            }

            return await ExecuteQueryAsync("GetVisitsByPatientIdProcedure", parameters);
        }

        public async Task<IEnumerable<Visit>> SearchByDateAsync(DateTime date, bool paginated = false, int perPage = 10, int page = 1)
        {
            var parameters = new Dictionary<string, object> { { "p_date", date } };

            if (paginated)
            {
                parameters.Add("p_per_page", perPage);
                parameters.Add("p_page", page);

                return await ExecuteQueryAsync("GetVisitsByDatePaginatedProcedure", parameters);
            }

            return await ExecuteQueryAsync("GetVisitsByDateProcedure", parameters);
        }

        public async Task<IEnumerable<Visit>> SearchByDateRangeAsync(DateTime startDate, DateTime endDate, bool paginated = false, int perPage = 10, int page = 1)
        {
            var parameters = new Dictionary<string, object>
            {
                {"p_start_date", startDate},
                {"p_end_date", endDate}
            };

            if (paginated)
            {
                parameters.Add("p_per_page", perPage);
                parameters.Add("p_page", page);

                return await ExecuteQueryAsync("GetVisitsByDateRangePaginatedProcedure", parameters);
            }

            return await ExecuteQueryAsync("GetVisitsByDateRangeProcedure", parameters);
        }

        public async Task<IEnumerable<Visit>> SearchByPatientIdAndDateRangeAsync(PatientId patientId, DateTime startDate, DateTime endDate, bool paginated = false, int perPage = 10, int page = 1)
        {
            var parameters = new Dictionary<string, object>
            {
                {"p_patient_id", patientId.Value},
                {"p_start_date", startDate},
                {"p_end_date", endDate}
            };

            if (paginated)
            {
                parameters.Add("p_per_page", perPage);
                parameters.Add("p_page", page);

                return await ExecuteQueryAsync("GetVisitsByPatientIdAndDateRangePaginatedProcedure", parameters);
            }

            return await ExecuteQueryAsync("GetVisitsByPatientIdAndDateRangeProcedure", parameters);
        }

        public async Task<IEnumerable<Visit>> SearchByPatientIdAndDateAsync(PatientId patientId, DateTime date, bool paginated = false, int perPage = 10, int page = 1)
        {
            var parameters = new Dictionary<string, object>
            {
                {"p_patient_id", patientId.Value},
                {"p_date", date}
            };

            if (paginated)
            {
                parameters.Add("p_per_page", perPage);
                parameters.Add("p_page", page);

                return await ExecuteQueryAsync("GetVisitsByPatientIdAndDatePaginatedProcedure", parameters);
            }

            return await ExecuteQueryAsync("GetVisitsByPatientIdAndDateProcedure", parameters);
        }
    }
}
