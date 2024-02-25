using GestorEnfermeriaJoyfe.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Domain.Visit.ValueObjects
{
    public sealed class VisitTraumaType : EnumValueObject<string>
    {
        public override HashSet<string> ValidValues { get; } = new() { "BUCODENTAL/MAXILOFACIAL", "CUERPO EXTRAÑO (INGESTA/OTROS)", "BRECHAS", "TEC", "CARA", "ROTURA DE GAFAS", "TRAUMATOLOGÍA MIEMBRO INFERIOR", "TRAUMATOLOGÍA MIEMBRO SUPERIOR", "OTROS ACCIDENTES" };

        public VisitTraumaType(string value) : base(value) { }
    }
}
