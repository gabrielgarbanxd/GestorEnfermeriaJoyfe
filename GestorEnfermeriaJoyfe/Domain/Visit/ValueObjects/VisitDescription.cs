using GestorEnfermeriaJoyfe.Domain.Shared;
using System;

namespace GestorEnfermeriaJoyfe.Domain.Visit.ValueObjects
{
    public sealed class VisitDescription : StringValueObject
    {
        private readonly int MaxLength = 65535;
        public VisitDescription(string value) : base(value) => EnsureValidLength(value);

        private void EnsureValidLength(string value)
        {
            if (value.Length > MaxLength)
            {
                throw new ArgumentException($"La descripción de la visita no puede ser mayor a {MaxLength} caracteres.");
            }
        }
    }
}
