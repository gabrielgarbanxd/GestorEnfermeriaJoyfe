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

            PatientQueryAdapter = new(patientRepository);
            PatientCommandAdapter = new(patientRepository);
        }

        public async Task<Response<List<Patient>>> GetAll() => await PatientQueryAdapter.GetAllPatients();

        public async Task<Response<List<Patient>>> GetAllPaginated(int perPage, int page) => await PatientQueryAdapter.GetAllPatientsPaginated(perPage, page);

        public async Task<Response<Patient>> Get(int id) => await PatientQueryAdapter.FindPatient(id);

        public async Task<Response<int>> Register(Patient patient) => await PatientCommandAdapter.CreatePatient(patient);

        public async Task<Response<bool>> Update(Patient patient) => await PatientCommandAdapter.UpdatePatient(patient);

        public async Task<Response<bool>> Delete(dynamic data) => await PatientCommandAdapter.DeletePatient(data);

    }
}
