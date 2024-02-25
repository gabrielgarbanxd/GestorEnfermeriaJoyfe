using GestorEnfermeriaJoyfe.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Domain.Visit.ValueObjects
{
    public sealed class VisitClassification : StringValueObject
    {
        private readonly int MaxLength = 255;
        public VisitClassification(string value) : base(value) => EnsureValidLength(value);

        private void EnsureValidLength(string value)
        {
            if (value.Length > MaxLength)
            {
                throw new ArgumentException($"La clasificación de la visita no puede ser mayor a {MaxLength} caracteres.");
            }
        }
    }
}
