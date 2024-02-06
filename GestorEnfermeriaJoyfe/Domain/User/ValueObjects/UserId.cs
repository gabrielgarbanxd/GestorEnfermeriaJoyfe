using GestorEnfermeriaJoyfe.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Domain.User.ValueObjects
{
    public class UserId : NumberValueObject
    {
        private const int MinUserIdValue = 1;

        public UserId(int value) : base(value)
        {
            if (value < MinUserIdValue)
            {
                throw new ArgumentException("El identificador del usuario no puede ser menor o igual a 0.");
            }
        }
    }
}
