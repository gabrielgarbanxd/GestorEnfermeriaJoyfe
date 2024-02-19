using GestorEnfermeriaJoyfe.Domain.Shared;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace GestorEnfermeriaJoyfe.Domain.User.ValueObjects
{
    public class UserPassword : StringValueObject
    {
        private const int MinLength = 8;
        public UserPassword(string value) : base(value)
        {
            EnsureIsNotEmpty(value);
            EnsureValidLength(value);

            //EnsureHasUpperCase(value);
            //EnsureHasLowerCase(value);
            //EnsureHasDigit(value);
            //EnsureHasSpecialCharacter(value);
        }

        public string GetHash()
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(Value));
                return BitConverter.ToString(hashedBytes).Replace("-", string.Empty);
            }
        }


        public bool VerifyPassword(string plainTextPassword)
        {
            string hashedInput = GetHashForPlainText(plainTextPassword);
            return string.Equals(hashedInput, Value, StringComparison.OrdinalIgnoreCase);
        }

        private string GetHashForPlainText(string plainTextPassword)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(plainTextPassword));
                return BitConverter.ToString(hashedBytes).Replace("-", string.Empty);
            }
        }

        private void EnsureIsNotEmpty(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("El nombre de usuario no puede estar vacío.");
            }
        }

        private void EnsureValidLength(string value)
        {
            if (value.Length < MinLength)
            {
                throw new ArgumentException("La contraseña no puede ser menor a 8 caracteres.");
            }
        }

        private bool EnsureHasUpperCase(string value)
        {
            return value.Any(char.IsUpper);
        }

        private bool EnsureHasLowerCase(string value)
        {
            return value.Any(char.IsLower);
        }

        private bool EnsureHasDigit(string value)
        {
            return value.Any(char.IsDigit);
        }

        private bool EnsureHasSpecialCharacter(string value)
        {
            string specialCharacters = "!@#$%^&*()-_+=<>?";

            return value.Intersect(specialCharacters).Any();
        }
    }
}
