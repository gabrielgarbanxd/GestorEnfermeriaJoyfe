using GestorEnfermeriaJoyfe.Domain.Shared;
using System;
using System.Text.RegularExpressions;

namespace GestorEnfermeriaJoyfe.Domain.User.ValueObjects
{
    public class UserEmail : StringValueObject
    {
        private const int MaxLength = 255;

        public UserEmail(string value) : base(value)
        {
            EnsureValidLength(value);
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

        private void EnsureValidLength(string value)
        {
            if (value.Length > MaxLength)
            {
                throw new ArgumentException($"El nombre de usuario no puede ser mayor a {MaxLength} caracteres.");
            }
        }
    }
}
