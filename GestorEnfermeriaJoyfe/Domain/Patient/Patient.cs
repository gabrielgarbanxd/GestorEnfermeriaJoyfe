using GestorEnfermeriaJoyfe.Domain.Patient.ValueObjects;

namespace GestorEnfermeriaJoyfe.Domain.Patient
{
    public class Patient
    {
        public PatientId Id { get; private set; }
        public PatientName Name { get; set; }
        public PatientLastName LastName { get; set; }
        public PatientLastName LastName2 { get; set; }
        public PatientCourse Course { get; set; }

        public Patient(PatientId? id, PatientName name, PatientLastName lastName, PatientLastName lastName2, PatientCourse course)
        {
            Id = id ?? new PatientId(0);
            Name = name;
            LastName = lastName;
            LastName2 = lastName2;
            Course = course;
        }

        public static Patient Create(PatientId? id, PatientName name, PatientLastName lastName, PatientLastName lastName2, PatientCourse course)
        {
            return new Patient(id, name, lastName, lastName2, course);
        }

        public static Patient FromPrimitives(int? id, string name, string lastName, string lastName2, string course)
        {
            return new Patient(new PatientId(id ?? 0), new PatientName(name), new PatientLastName(lastName), new PatientLastName(lastName2), new PatientCourse(course));
        }

        public void SetId(PatientId id)
        {
            if (Id.Value != 0)
            {
                throw new System.InvalidOperationException("El ID ya ha sido establecido.");
            }
            Id = id;
        }
    }
}
