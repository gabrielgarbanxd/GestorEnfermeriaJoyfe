using GestorEnfermeriaJoyfe.Domain.Shared;
using System;

namespace GestorEnfermeriaJoyfe.Domain.Cites.ValueObjects
{
    public class CitesNote : StringValueObject
    {
        private const int MaxLength = 255;

        public CitesNote(string value) : base(value)
        {
            EnsureNotEmpty(value);
            EnsureValidLength(value);
        }

        private void EnsureValidLength(string value)
        {
            if (value.Length > MaxLength)
            {
                throw new ArgumentException($"La nota de la cita no puede tener más de {MaxLength} caracteres.");
            }
        }

        private void EnsureNotEmpty(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("La nota de la cita no puede estar vacía.");
            }
        }
    }
}
