using GestorEnfermeriaJoyfe.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Domain.Visit.ValueObjects
{
    public sealed class VisitId : IdValueObject
    {
        public VisitId(int value) : base(value) { }
    }
}
