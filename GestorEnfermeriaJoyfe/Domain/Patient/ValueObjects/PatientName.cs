using GestorEnfermeriaJoyfe.Domain.Shared;
using System;
using System.Windows;

namespace GestorEnfermeriaJoyfe.Domain.Patient.ValueObjects
{
    public class PatientName : StringValueObject
    {
        private const int MaxPatientNameLength = 100;
        private const int MinPatientNameLength = 1;

        public PatientName(string value) : base(value)
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
            if (value.Length < MinPatientNameLength)
            {
                throw new ArgumentException($"El nombre del paciente no puede ser menor a {MinPatientNameLength} caracteres.");
            }

            if (value.Length > MaxPatientNameLength)
            {
                MessageBox.Show(value);
                throw new ArgumentException($"El nombre del paciente no puede ser mayor a {MaxPatientNameLength} caracteres.");
            }
        }  
    }
}
