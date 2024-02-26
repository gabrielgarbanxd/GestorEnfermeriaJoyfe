using GestorEnfermeriaJoyfe.ApplicationLayer.PatientApp;
using GestorEnfermeriaJoyfe.Domain.Patient;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Adapters.PatientAdapters
{
    public class PatientCommandAdapter : CommandAdapterBase
    {
        private readonly IPatientContract patientRepository;

        public PatientCommandAdapter(IPatientContract patientRepository) => this.patientRepository = patientRepository;

        public async Task<CommandResponse> CreatePatient(Patient patient)
        {
            return await RunCommand(async () =>
            {
                return await new PatientCreator(patientRepository).Run(patient);
            });
        }

        public async Task<CommandResponse> UpdatePatient(Patient patient)
        {
            return await RunCommand(async () =>
            {
                return await new PatientUpdater(patientRepository).Run(patient);
            });
        }

        public async Task<CommandResponse> DeletePatient(int id)
        {
            return await RunCommand(async () =>
            {
                return await new PatientDeleter(patientRepository).Run(id);
            });
        }

    }
}
