using GestorEnfermeriaJoyfe.Domain.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Adapters.PatientAdapters
{
    public class PatientController
    {
        public PatientController() { }

        public static async Task<Response<List<Patient>>> GetAll()
        {
            return await PatientQueryAdapter.GetAllPatients();
        }

        public static async Task<Response<List<Patient>>> GetAllPaginated(int perPage, int page)
        {
            return await PatientQueryAdapter.GetAllPatientsPaginated(perPage, page);
        }

        public static async Task<Response<Patient>> Get(int id)
        {
            return await PatientQueryAdapter.FindPatient(id);
        }

        public static async Task<Response<int>> Register(Patient patient)
        {
            return await PatientCommandAdapter.CreatePatient(patient);
        }

        public static async Task<Response<bool>> Update(Patient patient)
        {
            return await PatientCommandAdapter.UpdatePatient(patient);
        }

        public static async Task<Response<bool>> Delete(dynamic data)
        {
            return await PatientCommandAdapter.DeletePatient(data);
        }
    }
}
