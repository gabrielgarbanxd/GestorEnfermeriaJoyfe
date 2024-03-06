using GestorEnfermeriaJoyfe.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Domain.Visit.ValueObjects
{
    public class VisitPatientInfo : StringValueObject
    {
        public VisitPatientInfo(string value) : base(value)
        {
        }
    }
}
