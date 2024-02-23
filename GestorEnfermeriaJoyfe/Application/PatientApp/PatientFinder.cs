using GestorEnfermeriaJoyfe.Domain.Patient;
using GestorEnfermeriaJoyfe.Domain.Patient.ValueObjects;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Application.PatientApp
{
    public class PatientFinder
    {
        private readonly IPatientContract _patientRepository;

        public PatientFinder(IPatientContract patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<Patient> Run(int id)
        {
            return await _patientRepository.FindAsync(new PatientId(id));
        }
    }
}
