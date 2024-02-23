using GestorEnfermeriaJoyfe.Domain.Shared;
using System;

namespace GestorEnfermeriaJoyfe.Domain.User.ValueObjects
{
    public class UserId : NumberValueObject
    {
        private const int MinUserIdValue = 0;

        public UserId(int value) : base(value)
        {
            if (value < MinUserIdValue)
            {
                throw new ArgumentException("El identificador del usuario no puede ser menor o igual a 0.");
            }
        }
    }
}
