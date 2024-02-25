using GestorEnfermeriaJoyfe.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Domain.Visit.ValueObjects
{
    public sealed class VisitPlace : EnumValueObject<string>
    {
        public override HashSet<string> ValidValues { get; } = new() { "RECREO", "ED. FÍSICA", "CLASE", "NATACIÓN", "GUARDERÍA", "SEMANA DEPORTIVA", "DÍA VERDE", "EXTRAESCOLAR", "OTROS" };

        public VisitPlace(string value) : base(value) { }
    }
}
