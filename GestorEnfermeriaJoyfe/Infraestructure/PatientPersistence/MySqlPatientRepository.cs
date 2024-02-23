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

        public Task<List<Patient>> GetAllAsync(bool paginated = false, int perPage = 10, int page = 1)
        {
            if (paginated)
            {
                var parameters = new Dictionary<string, object>
                {
                    {"@PerPage", perPage},
                    {"@Page", page}
                };

                return ExecuteQueryAsync("GetAllPatientsPaginatedProcedure", parameters);
            }

            return ExecuteQueryAsync("GetAllPatientsProcedure");
        }

        public Task<Patient> FindAsync(PatientId patientId)
        {
            var result = ExecuteQueryAsync("GetPatientByIdProcedure", new Dictionary<string, object> { { "@Id", patientId.Value } });

            if (result.Count == 0)
            {
                throw new System.Exception("No se ha encontrado el paciente.");
            }

            return result[0];
        }

        public Task<int> AddAsync(Patient patient)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> UpdateAsync(Patient patient)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> DeleteAsync(PatientId patientId)
        {
            throw new System.NotImplementedException();
        }
    }
}
