using GestorEnfermeriaJoyfe.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Domain.ScheduledCiteRule.ValueObjects
{
    public class ScheduledCiteRuleName : StringValueObject
    {
        private const int MaxScheduledCiteRuleNameLength = 100;
        private const int MinScheduledCiteRuleNameLength = 1;

        public ScheduledCiteRuleName(string value) : base(value)
        {
            EnsureIsNotEmpty(value);
            EnsureValidLength(value);
        }

        private void EnsureIsNotEmpty(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("El nombre de la regla de cita programada no puede estar vacío.");
            }
        }

        private void EnsureValidLength(string value)
        {
            if (value.Length < MinScheduledCiteRuleNameLength)
            {
                throw new ArgumentException($"El nombre de la regla de cita programada no puede ser menor a {MinScheduledCiteRuleNameLength} caracteres.");
            }

            if (value.Length > MaxScheduledCiteRuleNameLength)
            {
                throw new ArgumentException($"El nombre de la regla de cita programada no puede ser mayor a {MaxScheduledCiteRuleNameLength} caracteres.");
            }
        }
    }
}
