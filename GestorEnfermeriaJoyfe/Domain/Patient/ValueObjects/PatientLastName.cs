using GestorEnfermeriaJoyfe.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Domain.Patient.ValueObjects
{
    public class PatientLastName : StringValueObject
    {
        private const int MinLength = 3;
        private const int MaxLength = 50;

        public PatientLastName(string value) : base(value)
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
                throw new ArgumentException($"El apellido del paciente no puede ser menor a {MinLength} caracteres.");
            }
            if (value.Length > MaxLength)
            {
                throw new ArgumentException($"El apellido del paciente no puede ser mayor a {MaxLength} caracteres.");
            }
        }
    }
}
