using GestorEnfermeriaJoyfe.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Domain.Patient.ValueObjects
{
    public class PatientId : NumberValueObject
    {
        private const int MinPatientIdValue = 0;

        public PatientId(int value) : base(value)
        {
            if (value < MinPatientIdValue)
            {
                throw new ArgumentException("El identificador del paciente no puede ser menor o igual a 0.");
            }
        }
    }
}
