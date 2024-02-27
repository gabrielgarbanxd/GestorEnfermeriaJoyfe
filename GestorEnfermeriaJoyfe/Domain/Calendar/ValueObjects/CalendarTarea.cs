using GestorEnfermeriaJoyfe.Domain.Shared;
using System;
using System.Text.RegularExpressions;

namespace GestorEnfermeriaJoyfe.Domain.Calendar.ValueObjects
{
    public class CalendarTarea : StringValueObject
    {
        private const int MaxLength = 255;

        public CalendarTarea(string value) : base(value)
        {
            EnsureValidLength(value);
        }

        private void EnsureValidLength(string value)
        {
            if (value.Length > MaxLength)
            {
                throw new ArgumentException($"La tarea del calendario no puede ser mayor a {MaxLength} caracteres.");
            }
        }


        private void EnsureNotEmpty(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("La tarea del calendario no puede estar vacía.");
            }
        }









    }
}
