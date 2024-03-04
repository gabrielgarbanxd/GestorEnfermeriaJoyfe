using GestorEnfermeriaJoyfe.Domain.Patient.ValueObjects;
using GestorEnfermeriaJoyfe.Domain.Visit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.ApplicationLayer.VisitApp.SearchBy
{
    public class VisitsByPatientIdSearcher
    {
        private readonly IVisitContract _visitContract;

        public VisitsByPatientIdSearcher(IVisitContract visitContract) => _visitContract = visitContract;

        public async Task<IEnumerable<Visit>> Run(int patientId, bool paginated = false, int perPage = 10, int page = 1)
        {
            if (paginated)
            {
                if (perPage <= 0 || page <= 0)
                {
                    throw new ArgumentException("El número de página y el número de elementos por página deben ser mayores a 0.");
                }

                return await _visitContract.SearchByPatientIdAsync(new PatientId(patientId), paginated, perPage, page);
            }

            return await _visitContract.SearchByPatientIdAsync(new PatientId(patientId));
        }
    }
}
