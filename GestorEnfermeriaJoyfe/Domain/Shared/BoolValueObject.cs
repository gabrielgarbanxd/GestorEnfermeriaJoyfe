using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Domain.Shared
{
    public abstract class BoolValueObject : ValueObject<bool>
    {
        public BoolValueObject(bool value) : base(value) { }

       public int ToInt() => Value ? 1 : 0;
       
        public string ToYesNoString() => Value ? "Sí" : "No";

        public string ToCheckString => Value ? "✓" : "✗";


    }
}
