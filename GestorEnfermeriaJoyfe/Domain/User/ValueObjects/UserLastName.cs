using GestorEnfermeriaJoyfe.Domain.Shared;
using System;

namespace GestorEnfermeriaJoyfe.Domain.User.ValueObjects
{
    public class UserLastName : StringValueObject
    {
        private const int MinLength = 3;
        private const int MaxLength = 50;

        public UserLastName(string value) : base(value)
        {
            EnsureIsNotEmpty(value);
            EnsureValidLength(value);
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
                throw new ArgumentException($"El nombre de usuario no puede ser menor a {MinLength} caracteres.");
            }

            if (value.Length > MaxLength)
            {
                throw new ArgumentException($"El nombre de usuario no puede ser mayor a {MaxLength} caracteres.");
            }
        }
    }
}
