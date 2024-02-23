using GestorEnfermeriaJoyfe.Domain.Shared;
using System;

namespace GestorEnfermeriaJoyfe.Domain.Patient.ValueObjects
{
    public class PatientCourse : StringValueObject
    {
        private const int MinLength = 1;
        private const int MaxLength = 100;

        public PatientCourse(string value) : base(value)
        {
            EnsureIsNotEmpty(value);
            EnsureValidLength(value);
        }

        private void EnsureIsNotEmpty(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("El curso del paciente no puede estar vacío.");
            }
        }

        private void EnsureValidLength(string value)
        {
            if (value.Length < MinLength)
            {
                throw new ArgumentException($"El curso del paciente no puede ser menor a {MinLength} caracteres.");
            }
            if (value.Length > MaxLength)
            {
                throw new ArgumentException($"El curso del paciente no puede ser mayor a {MaxLength} caracteres.");
            }
        }   
    }
}
