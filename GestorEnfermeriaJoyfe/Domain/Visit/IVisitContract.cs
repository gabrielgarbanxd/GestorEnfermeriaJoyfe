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
        Task<List<Visit>> SearchByPatientIdAsync(PatientId patientId, bool paginated = false, int perPage = 10, int page = 1);
        Task<List<Visit>> SearchByDateAsync(DateTime date, bool paginated = false, int perPage = 10, int page = 1);
        Task<List<Visit>> SearchByDateRangeAsync(DateTime startDate, DateTime endDate, bool paginated = false, int perPage = 10, int page = 1);
        Task<List<Visit>> SearchByPatientIdAndDateRangeAsync(PatientId patientId, DateTime startDate, DateTime endDate, bool paginated = false, int perPage = 10, int page = 1);
        Task<List<Visit>> SearchByPatientIdAndDateAsync(PatientId patientId, DateTime date, bool paginated = false, int perPage = 10, int page = 1);
    }
}
