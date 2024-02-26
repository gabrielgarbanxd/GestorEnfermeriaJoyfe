using GestorEnfermeriaJoyfe.Domain.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Adapters.PatientAdapters
{
    public class PatientResponse : ResponseBase<IEnumerable<Patient>>
    {
        public PatientResponse(bool success, string? message, IEnumerable<Patient>? data) : base(success, message, data)
        {
        }

        public PatientResponse(bool success, string? message) : base(success, message)
        {
        }

        public PatientResponse()
        {
        }

        public override PatientResponse Ok(string? message = null, IEnumerable<Patient>? data = default)
        {
            return new PatientResponse(true, message, data);
        }

        public override PatientResponse Fail(string? message = null)
        {
            return new PatientResponse(false, message);
        }
    }
}
