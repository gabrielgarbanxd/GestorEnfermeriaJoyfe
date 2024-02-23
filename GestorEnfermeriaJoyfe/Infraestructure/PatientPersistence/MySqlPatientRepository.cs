using GestorEnfermeriaJoyfe.Domain.Patient;
using GestorEnfermeriaJoyfe.Domain.Patient.ValueObjects;
using GestorEnfermeriaJoyfe.Infraestructure.Shared;
using System.Collections.Generic;
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

        public async Task<List<Patient>> GetAllAsync(bool paginated = false, int perPage = 10, int page = 1)
        {
            if (paginated)
            {
                var parameters = new Dictionary<string, object>
                {
                    {"@PerPage", perPage},
                    {"@Page", page}
                };

                return await ExecuteQueryAsync("GetAllPatientsPaginatedProcedure", parameters);
            }

            return await ExecuteQueryAsync("GetAllPatientsProcedure");
        }

        public async Task<Patient> FindAsync(PatientId patientId)
        {
            var result = await ExecuteQueryAsync("GetPatientByIdProcedure", new Dictionary<string, object> { { "@Id", patientId.Value } });

            if (result.Count == 0)
            {
                throw new System.Exception("No se ha encontrado el paciente.");
            }

            return result[0];
        }

        public async Task<int> AddAsync(Patient patient)
        {
            var parameters = new Dictionary<string, object>
            {
                {"@Name", patient.Name.Value},
                {"@LastName", patient.LastName.Value},
                {"@LastName2", patient.LastName2.Value},
                {"@Course", patient.Course.Value}
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
                {"@Id", patient.Id.Value},
                {"@Name", patient.Name.Value},
                {"@LastName", patient.LastName.Value},
                {"@LastName2", patient.LastName2.Value},
                {"@Course", patient.Course.Value}
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
            var result = await ExecuteNonQueryAsync("DeletePatientProcedure", new Dictionary<string, object> { { "@Id", patientId.Value } });

            if (result <= 0)
            {
                throw new System.Exception("No se ha podido eliminar el paciente.");
            }

            return result;
        }
    }
}
