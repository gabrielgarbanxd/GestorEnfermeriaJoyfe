using GestorEnfermeriaJoyfe.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Domain.Visit.ValueObjects
{
    public sealed class VisitType : EnumValueObject<string>
    {
        public override HashSet<string> ValidValues { get; set; } = new(){ "Agudo", "Crónico" };

        public VisitType(string value) : base(value) { }
    }
}
