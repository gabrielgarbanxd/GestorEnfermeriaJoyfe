using GestorEnfermeriaJoyfe.Domain.Patient;
using GestorEnfermeriaJoyfe.Domain.Patient.ValueObjects;
using GestorEnfermeriaJoyfe.Infraestructure.Shared;
using System.Data;

namespace GestorEnfermeriaJoyfe.Infraestructure.PatientPersistence
{
    public class PatientMapper : IObjectMapper<Patient>
    {
        public Patient Map(IDataReader reader)
        {
            int id = reader.GetInt32(reader.GetOrdinal("id"));
            string name = reader.GetString(reader.GetOrdinal("name"));
            string lastName = reader.GetString(reader.GetOrdinal("last_name"));
            string lastName2 = reader.GetString(reader.GetOrdinal("last_name2"));
            string course = reader.GetString(reader.GetOrdinal("course"));

            Patient patient = new(
                new PatientId(id),
                new PatientName(name),
                new PatientLastName(lastName),
                new PatientLastName(lastName2),
                new PatientCourse(course)
            );

            return patient;
        }
    }
}