using GestorEnfermeriaJoyfe.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Domain.Cite.ValueObjects
{
    public class CitePatientInfo : StringValueObject
    {
        public CitePatientInfo(string value) : base(value)
        {
        }
    }
}
