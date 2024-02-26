using GestorEnfermeriaJoyfe.Domain.Patient;
using GestorEnfermeriaJoyfe.Domain.Patient.ValueObjects;
using GestorEnfermeriaJoyfe.Infraestructure.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Infraestructure.PatientPersistence
{
    public class MySqlPatientRepository : MySqlRepositoryBase<Patient>, IPatientContract
    {

        public MySqlPatientRepository(PatientMapper mapper) : base(mapper)
        {
        }

        public MySqlPatientRepository()
        {
        }

        public async Task<IEnumerable<Patient>> GetAllAsync(bool paginated = false, int perPage = 10, int page = 1)
        {
            if (paginated)
            {
                var parameters = new Dictionary<string, object>
                {
                    {"p_per_page", perPage},
                    {"p_page", page}
                };

                return await ExecuteQueryAsync("GetAllPatientsPaginatedProcedure", parameters);
            }

            return await ExecuteQueryAsync("GetAllPatientsProcedure");
        }

        public async Task<Patient> FindAsync(PatientId patientId)
        {
            var result = await ExecuteQueryAsync("GetPatientByIdProcedure", new Dictionary<string, object> { { "p_id", patientId.Value } });

            if (!result.Any())
            {
                throw new System.Exception("No se ha encontrado el paciente.");
            }

            return result.First();
        }

        public async Task<int> AddAsync(Patient patient)
        {
            var parameters = new Dictionary<string, object>
            {
                {"p_name", patient.Name.Value},
                {"p_last_name", patient.LastName.Value},
                {"p_last_name2", patient.LastName2.Value},
                {"p_course", patient.Course.Value}
            };
            var result = await ExecuteNonQueryAsync("CreatePatientProcedure", parameters);

            if (result <= 0)
            {
                throw new System.Exception("No se ha podido crear el paciente.");
            }

            return result;
        }

        public async Task<int> UpdateAsync(Patient patient)
        {
            var parameters = new Dictionary<string, object>
            {
                {"p_id", patient.Id.Value},
                {"p_name", patient.Name.Value},
                {"p_last_name", patient.LastName.Value},
                {"p_last_name2", patient.LastName2.Value},
                {"p_course", patient.Course.Value}
            };
            var result = await ExecuteNonQueryAsync("UpdatePatientProcedure", parameters);

            if (result <= 0)
            {
                throw new System.Exception("No se ha podido actualizar el paciente.");
            }

            return result;
        }

        public async Task<int> DeleteAsync(PatientId patientId)
        {
            var result = await ExecuteNonQueryAsync("DeletePatientProcedure", new Dictionary<string, object> { { "p_id", patientId.Value } });

            if (result <= 0)
            {
                throw new System.Exception("No se ha podido eliminar el paciente.");
            }

            return result;
        }
    }
}
