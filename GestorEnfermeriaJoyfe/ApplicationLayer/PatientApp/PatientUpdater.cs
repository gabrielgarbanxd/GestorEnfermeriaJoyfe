using GestorEnfermeriaJoyfe.Domain.Patient;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.ApplicationLayer.PatientApp
{
    public class PatientUpdater
    {
        private readonly IPatientContract _patientRepository;

        public PatientUpdater(IPatientContract patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<bool> Run(Patient patient)
        {
            return await _patientRepository.UpdateAsync(patient) > 0;
        }
    }
}
