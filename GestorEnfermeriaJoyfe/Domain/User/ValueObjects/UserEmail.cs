using GestorEnfermeriaJoyfe.Domain.Shared;
using System;
using System.Text.RegularExpressions;

namespace GestorEnfermeriaJoyfe.Domain.User.ValueObjects
{
    public class UserEmail : StringValueObject
    {
        public UserEmail(string value) : base(value)
        {
            EnsureValidEmailFormat(value);
        }

        private void EnsureValidEmailFormat(string email)
        {
            string emailRegexPattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";

            if (!Regex.IsMatch(email, emailRegexPattern))
            {
                throw new ArgumentException("El formato del correo electrónico no es válido.");
            }
        }

        public string NormalizeEmail()
        {
            return Value.ToLowerInvariant();
        }
    }
}
