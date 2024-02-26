using System.Collections.Generic;
using GestorEnfermeriaJoyfe.Domain.Patient;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.ApplicationLayer.PatientApp
{
    public class PatientLister
    {
        private readonly IPatientContract _patientRepository;

        public PatientLister(IPatientContract patientRepository) => _patientRepository = patientRepository;

        public async Task<IEnumerable<Patient>> Run(bool paginated = false, int perPage = 10, int page = 1)
        {
            if (paginated)
            {
                if (perPage <= 0 || page <= 0)
                {
                    throw new System.ArgumentException("El número de página y el número de elementos por página deben ser mayores a 0.");
                }

                return await _patientRepository.GetAllAsync(paginated, perPage, page);
            }

            return await _patientRepository.GetAllAsync();
        }
    }
}
