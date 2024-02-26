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
    public class PatientQueryAdapter
    {
        private readonly IPatientContract patientRepository;

        public PatientQueryAdapter(IPatientContract patientRepository)
        {
            this.patientRepository = patientRepository;
        }

        public async Task<Response<Patient>> FindPatient(int id)
        {
            try
            {
                PatientFinder patientFinder = new(patientRepository);

                var patient = await patientFinder.Run(id);

                return patient != null ? Response<Patient>.Ok("Paciente encontrado", patient) : Response<Patient>.Fail("Paciente no encontrado");
            }
            catch (Exception e)
            {
                return Response<Patient>.Fail(e.Message);
            }
        }

        public async Task<Response<List<Patient>>> GetAllPatients()
        {
            try
            {
                PatientLister patientLister = new(patientRepository);

                var patients = await patientLister.Run();

                return patients.Count > 0 ? Response<List<Patient>>.Ok("Pacientes encontrados", patients) : Response<List<Patient>>.Fail("No se encontraron pacientes");
            }
            catch (Exception e)
            {
                return Response<List<Patient>>.Fail(e.Message);
            }
        }

        public async Task<Response<List<Patient>>> GetAllPatientsPaginated(int perPage, int page)
        {
            try
            {
                PatientLister patientLister = new(patientRepository);

                var patients = await patientLister.Run(true, perPage, page);

                return patients.Count > 0 ? Response<List<Patient>>.Ok("Pacientes encontrados", patients) : Response<List<Patient>>.Fail("No se encontraron pacientes");
            }
            catch (Exception e)
            {
                return Response<List<Patient>>.Fail(e.Message);
            }

        }
    }
}
