using GestorEnfermeriaJoyfe.Domain.Patient;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.ApplicationLayer.PatientApp
{
    public class PatientUpdater
    {
        private readonly IPatientContract _patientRepository;

        public PatientUpdater(IPatientContract patientRepository) => _patientRepository = patientRepository;

        public async Task<int> Run(Patient patient) => await _patientRepository.UpdateAsync(patient);
    }
}
