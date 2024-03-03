using GestorEnfermeriaJoyfe.Domain.Cite.ValueObjects;
using GestorEnfermeriaJoyfe.Domain.Cite;
using GestorEnfermeriaJoyfe.Infraestructure.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace GestorEnfermeriaJoyfe.Infraestructure.CitePersistence
{
    public class MySqlCiteRepository : MySqlRepositoryBase<Cite>, ICiteContract
    {
        public MySqlCiteRepository(CiteMapper mapper) : base(mapper)
        {
        }

        public MySqlCiteRepository()
        {
        }

        public async Task<IEnumerable<Cite>> GetAllAsync(bool paginated = false, int perPage = 10, int page = 1)
        {
            if (paginated)
            {
                var parameters = new Dictionary<string, object>
                {
                    {"p_per_page", perPage},
                    {"p_page", page}
                };

                return await ExecuteQueryAsync("GetAllCitesPaginatedProcedure", parameters);
            }

            return await ExecuteQueryAsync("GetAllCitesProcedure");
        }

        public async Task<Cite> FindAsync(CiteId citeId)
        {
            var result = await ExecuteQueryAsync("GetCiteByIdProcedure", new Dictionary<string, object> { { "p_id", citeId.Value } });

            if (!result.Any())
            {
                throw new Exception("No se ha encontrado la cita.");
            }

            return result.First();
        }

        public async Task<int> AddAsync(Cite cite)
        {
            var parameters = new Dictionary<string, object>
            {
                {"p_date", cite.Date.Value},
                {"p_patient_id", cite.PatientId.Value},
                {"p_note", cite.Note.Value}
            };

            var result = await ExecuteNonQueryAsync("CreateCiteProcedure", parameters);

            if (result <= 0)
            {
                throw new Exception("No se ha podido crear la cita.");
            }

            return result;
        }

        public async Task<int> UpdateAsync(Cite cite)
        {
            var parameters = new Dictionary<string, object>
            {
                {"p_id", cite.Id.Value},
                {"p_date", cite.Date.Value},
                {"p_patient_id", cite.PatientId.Value},
                {"p_note", cite.Note.Value}
            };

            var result = await ExecuteNonQueryAsync("UpdateCiteProcedure", parameters);

            if (result <= 0)
            {
                throw new Exception("No se ha podido actualizar la cita.");
            }

            return result;
        }

        public async Task<int> DeleteAsync(CiteId citeId)
        {
            var result = await ExecuteNonQueryAsync("DeleteCiteProcedure", new Dictionary<string, object> { { "p_id", citeId.Value } });

            if (result <= 0)
            {
                throw new Exception("No se ha podido eliminar la cita.");
            }

            return result;
        }

        public async Task<IEnumerable<Cite>> GetCitesByPatientIdAsync(int patientId, bool paginated = false, int perPage = 10, int page = 1)
        {
            var parameters = new Dictionary<string, object>
            {
                {"p_patient_id", patientId},
                {"p_per_page", perPage},
                {"p_page", page}
            };

            if (paginated)
            {
                return await ExecuteQueryAsync("GetCitesByPatientIdPaginatedProcedure", parameters);
            }

            return await ExecuteQueryAsync("GetCitesByPatientIdProcedure", parameters);
        }

        public async Task<IEnumerable<Cite>> GetCitesByDayAsync(DateTime date, bool paginated = false, int perPage = 10, int page = 1)
        {
            var parameters = new Dictionary<string, object>
            {
                {"p_date", date},
                {"p_per_page", perPage},
                {"p_page", page}
            };

            if (paginated)
            {
                return await ExecuteQueryAsync("GetCitesByDayPaginatedProcedure", parameters);
            }

            return await ExecuteQueryAsync("GetCitesByDayProcedure", parameters);
        }

        public async Task<IEnumerable<Cite>> GetCitesByDayAndPatientIdAsync(int patientId, DateTime date, bool paginated = false, int perPage = 10, int page = 1)
        {
            var parameters = new Dictionary<string, object>
            {
                {"p_patient_id", patientId},
                {"p_date", date},
                {"p_per_page", perPage},
                {"p_page", page}
            };

            if (paginated)
            {
                return await ExecuteQueryAsync("GetCitesByDayAndPatientIdPaginatedProcedure", parameters);
            }

            return await ExecuteQueryAsync("GetCitesByDayAndPatientIdProcedure", parameters);
        }

        public async Task<IEnumerable<Cite>> GetCitesByDayRangeAsync(DateTime startDate, DateTime endDate, bool paginated = false, int perPage = 10, int page = 1)
        {
            var parameters = new Dictionary<string, object>
            {
                {"p_start_date", startDate},
                {"p_end_date", endDate},
                {"p_per_page", perPage},
                {"p_page", page}
            };

            if (paginated)
            {
                return await ExecuteQueryAsync("GetCitesByDayRangePaginatedProcedure", parameters);
            }

            return await ExecuteQueryAsync("GetCitesByDayRangeProcedure", parameters);
        }

        public async Task<IEnumerable<Cite>> GetCitesByPatientIdAndDayAsync(int patientId, DateTime date, bool paginated = false, int perPage = 10, int page = 1)
        {
            var parameters = new Dictionary<string, object>
            {
                {"p_patient_id", patientId},
                {"p_date", date},
                {"p_per_page", perPage},
                {"p_page", page}
            };

            if (paginated)
            {
                return await ExecuteQueryAsync("GetCitesByPatientIdAndDayPaginatedProcedure", parameters);
            }

            return await ExecuteQueryAsync("GetCitesByPatientIdAndDayProcedure", parameters);
        }

        public async Task<IEnumerable<Cite>> GetCitesByPatientIdAndDayRangeAsync(int patientId, DateTime startDate, DateTime endDate, bool paginated = false, int perPage = 10, int page = 1)
        {
            var parameters = new Dictionary<string, object>
            {
                {"p_patient_id", patientId},
                {"p_start_date", startDate},
                {"p_end_date", endDate},
                {"p_per_page", perPage},
                {"p_page", page}
            };

            if (paginated)
            {
                return await ExecuteQueryAsync("GetCitesByPatientIdAndDayRangePaginatedProcedure", parameters);
            }

            return await ExecuteQueryAsync("GetCitesByPatientIdAndDayRangeProcedure", parameters);
        }
    }
}

