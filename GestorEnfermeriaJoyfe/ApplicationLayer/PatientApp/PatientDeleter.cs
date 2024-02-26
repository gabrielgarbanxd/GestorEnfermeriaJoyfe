using GestorEnfermeriaJoyfe.Domain.Patient;
using GestorEnfermeriaJoyfe.Domain.Patient.ValueObjects;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.ApplicationLayer.PatientApp
{
    public class PatientDeleter
    {
        private readonly IPatientContract _patientRepository;

        public PatientDeleter(IPatientContract patientRepository) => _patientRepository = patientRepository;

        public async Task<int> Run(int id) => await _patientRepository.DeleteAsync(new PatientId(id));
    }
}
