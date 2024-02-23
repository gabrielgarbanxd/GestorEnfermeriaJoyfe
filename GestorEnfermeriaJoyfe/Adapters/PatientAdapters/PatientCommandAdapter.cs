using GestorEnfermeriaJoyfe.Application.PatientApp;
using GestorEnfermeriaJoyfe.Domain.Patient;
using GestorEnfermeriaJoyfe.Infraestructure.PatientPersistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Adapters.PatientAdapters
{
    public class PatientCommandAdapter
    {
        private static readonly MySqlPatientRepository patientRepository = new();

        public static async Task<Response<int>> CreatePatient(Patient patient)
        {
            try
            {
                PatientCreator patientRegister = new(patientRepository);

                int newPatientId = await patientRegister.Run(patient);

                return Response<int>.Ok("Paciente registrado", newPatientId);
            }
            catch (Exception e)
            {
                return Response<int>.Fail(e.Message);
            }
        }

        public static async Task<Response<bool>> DeletePatient(int id)
        {
            try
            {
                PatientDeleter patientDeleter = new(patientRepository);

                bool deleted = await patientDeleter.Run(id);

                return deleted ? Response<bool>.Ok("Paciente eliminado") : Response<bool>.Fail("Paciente no eliminado");
            }
            catch (Exception e)
            {
                return Response<bool>.Fail(e.Message);
            }
        }

        public static async Task<Response<bool>> UpdatePatient(Patient patient)
        {
            try
            {
                PatientUpdater patientUpdater = new(patientRepository);
                bool updated = await patientUpdater.Run(patient);

                return updated ? Response<bool>.Ok("Paciente actualizado") : Response<bool>.Fail("Paciente no actualizado");
            }
            catch (Exception e)
            {
                return Response<bool>.Fail(e.Message);
            }
        }
    }
}
