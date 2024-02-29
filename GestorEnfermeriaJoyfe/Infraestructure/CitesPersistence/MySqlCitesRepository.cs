using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestorEnfermeriaJoyfe.Domain.Cites;
using GestorEnfermeriaJoyfe.Domain.Cites.ValueObjects;
using GestorEnfermeriaJoyfe.Infraestructure.Shared;

namespace GestorEnfermeriaJoyfe.Infraestructure.CitesPersistence
{
    public class MySqlCitesRepository : MySqlRepositoryBase<Cite>, ICitesContract
    {
        public MySqlCitesRepository(CitesMapper mapper) : base(mapper)
        {
        }

        public MySqlCitesRepository()
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

        // Corregir el parámetro de entrada del método FindAsync para que coincida con la firma de la interfaz
        public async Task<Cite> FindAsync(int citeId)
        {
            var result = await ExecuteQueryAsync("GetCiteByIdProcedure", new Dictionary<string, object> { { "p_id", citeId } });

            if (!result.Any())
            {
                throw new Exception("No se ha encontrado la cita.");
            }

            return result.First();
        }


        public async Task<int> CreateAsync(Cite cite)
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
                throw new Exception("No se ha podido crear la cita.");
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
                throw new Exception("No se ha podido actualizar la cita.");
            }

            return result;
        }

        public async Task<int> DeleteAsync(int citeId)
        {
            var result = await ExecuteNonQueryAsync("DeleteCiteProcedure", new Dictionary<string, object> { { "p_id", citeId } });

            if (result <= 0)
            {
                throw new Exception("No se ha podido eliminar la cita.");
            }

            return result;
        }
    }
}
