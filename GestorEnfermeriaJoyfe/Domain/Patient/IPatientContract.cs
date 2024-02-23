using GestorEnfermeriaJoyfe.Domain.Patient.ValueObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Domain.Patient
{
    public interface IPatientContract
    {
        Task<List<Patient>> GetAllAsync(bool paginated = false, int perPage = 10, int page = 1);
        Task<Patient> FindAsync(PatientId patientId);
        Task<int> AddAsync(Patient patient);
        Task<int> UpdateAsync(Patient patient);
        Task<int> DeleteAsync(PatientId patientId);
    }
}
