using GestorEnfermeriaJoyfe.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Domain.VisitTemplate.ValueObjects
{
    public class VisitTemplateName : StringValueObject
    {
        private readonly int minLength = 1;
        private readonly int maxLength = 50;

        public VisitTemplateName(string value) : base(value) 
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
            if (value.Length < minLength)
            {
                throw new ArgumentException($"El nombre de la plantilla de visita no puede ser menor a {minLength} caracteres.");
            }

            if (value.Length > maxLength)
            {
                throw new ArgumentException($"El nombre de la plantilla de visita no puede ser mayor a {maxLength} caracteres.");
            }
        }
    }
}
