using GestorEnfermeriaJoyfe.Domain.Patient;
using GestorEnfermeriaJoyfe.Infraestructure.PatientPersistence;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Adapters.PatientAdapters
{
    public class PatientController
    {
        private readonly PatientMapper patientMapper;
        private readonly MySqlPatientRepository patientRepository;


        private readonly PatientQueryAdapter PatientQueryAdapter;
        private readonly PatientCommandAdapter PatientCommandAdapter;

        public PatientController()
        {
            patientMapper = new();
            patientRepository = new(patientMapper);

            PatientQueryAdapter = new(patientRepository, new());
            PatientCommandAdapter = new(patientRepository);
        }

        // ================== QUERYS ==================

        public async Task<PatientResponse> GetAll() => await PatientQueryAdapter.GetAllPatients();
        public async Task<PatientResponse> GetAllPaginated(int perPage, int page) => await PatientQueryAdapter.GetAllPatientsPaginated(perPage, page);
        public async Task<PatientResponse> Get(int id) => await PatientQueryAdapter.FindPatient(id);

        // ================== COMMANDS ==================
        public async Task<CommandResponse> Register(Patient patient) => await PatientCommandAdapter.CreatePatient(patient);
        public async Task<CommandResponse> Update(Patient patient) => await PatientCommandAdapter.UpdatePatient(patient);
        public async Task<CommandResponse> Delete(int id) => await PatientCommandAdapter.DeletePatient(id);

    }
}
