using GestorEnfermeriaJoyfe.Domain.Patient.ValueObjects;
using GestorEnfermeriaJoyfe.Domain.Visit.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Domain.Visit
{
    public interface IVisitContract
    {
        Task<List<Visit>> GetAllAsync(bool paginated = false, int perPage = 10, int page = 1);
        Task<Visit> FindAsync(VisitId visitId);
        Task<int> CreateAsync(Visit visit);
        Task<int> UpdateAsync(Visit visit);
        Task<int> DeleteAsync(VisitId visitId);
        Task<List<Visit>> SearchAsyncByPatientId(PatientId patientId, bool paginated = false, int perPage = 10, int page = 1);
        Task<List<Visit>> SearchAsyncByDate(DateTime date, bool paginated = false, int perPage = 10, int page = 1);
    }
}
