using GestorEnfermeriaJoyfe.ApplicationLayer.PatientApp;
using GestorEnfermeriaJoyfe.Domain.Patient;
using GestorEnfermeriaJoyfe.Infraestructure.PatientPersistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Adapters.PatientAdapters
{
    public class PatientQueryAdapter : QueryAdapterBase<PatientResponse, IEnumerable<Patient>>
    {
        private readonly IPatientContract patientRepository;

        public PatientQueryAdapter(IPatientContract patientRepository, PatientResponse response) : base(response)
        {
            this.patientRepository = patientRepository;
        }

        public async Task<PatientResponse> FindPatient(int id)
        {
            return await RunQuery(async () =>
            {
                return new List<Patient> { await new PatientFinder(patientRepository).Run(id) };
            });
        }

        public async Task<PatientResponse> GetAllPatients()
        {
            return await RunQuery(async () =>
            {
                return await new PatientLister(patientRepository).Run();
            });
        }

        public async Task<PatientResponse> GetAllPatientsPaginated(int perPage, int page)
        {
            return await RunQuery(async () =>
            {
                return await new PatientLister(patientRepository).Run(true, perPage, page);
            });
        }
    }
}
