using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Domain.Shared
{
    public abstract class IdValueObject : NumberValueObject
    {
        private readonly int MinIdValue = 0;
        protected IdValueObject(int value) : base(value) 
        {
            if (value < MinIdValue)
            {
                throw new ArgumentException("El identificador unico no puede ser menor a 0.");
            }
        }
    }
}
