using GestorEnfermeriaJoyfe.Domain.Cite.ValueObjects;
using GestorEnfermeriaJoyfe.Domain.Cite;
using GestorEnfermeriaJoyfe.Infraestructure.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                throw new System.Exception("No se ha encontrado la cita.");
            }

            return result.First();
        }

        public async Task<int> AddAsync(Cite cite)
        {
            var parameters = new Dictionary<string, object>
            {
                {"p_patient_id", cite.PatientId},
                {"p_note", cite.Note},
                {"p_visit_id", cite.VisitId},
                {"p_date", cite.Date}
            };

            var result = await ExecuteNonQueryAsync("CreateCiteProcedure", parameters);

            if (result <= 0)
            {
                throw new System.Exception("No se ha podido crear la cita.");
            }

            return result;
        }

        public async Task<int> UpdateAsync(Cite cite)
        {
            var parameters = new Dictionary<string, object>
            {
                {"p_id", cite.Id.Value},
                {"p_patient_id", cite.PatientId},
                {"p_note", cite.Note},
                {"p_visit_id", cite.VisitId},
                {"p_date", cite.Date}
            };

            var result = await ExecuteNonQueryAsync("UpdateCiteProcedure", parameters);

            if (result <= 0)
            {
                throw new System.Exception("No se ha podido actualizar la cita.");
            }
            return result;
        }

        public async Task<int> DeleteAsync(CiteId citeId)
        {
            var result = await ExecuteNonQueryAsync("DeleteCiteProcedure", new Dictionary<string, object> { { "p_id", citeId.Value } });

            if (result <= 0)
            {
                throw new System.Exception("No se ha podido eliminar la cita.");
            }

            return result;
        }
    }
}
