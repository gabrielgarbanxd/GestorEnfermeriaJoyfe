using GestorEnfermeriaJoyfe.Domain.Patient;
using GestorEnfermeriaJoyfe.Domain.Patient.ValueObjects;
using GestorEnfermeriaJoyfe.Domain.ScheduledCiteRule.ValueObjects;
using System;

namespace GestorEnfermeriaJoyfe.Domain.ScheduledCiteRule
{
    public class ScheduledCiteRule
    {
        public ScheduledCiteRuleId Id { get; private set; }
        public ScheduledCiteRuleName Name { get; set; }
        public ScheduledCiteRuleHour Hour { get; set; }
        public ScheduledCiteRuleStartDate StartDate { get; set; }
        public ScheduledCiteRuleEndDate EndDate { get; set; }
        public ScheduledCiteRuleDay Lunes { get; set; }
        public ScheduledCiteRuleDay Martes { get; set; }
        public ScheduledCiteRuleDay Miercoles { get; set; }
        public ScheduledCiteRuleDay Jueves { get; set; }
        public ScheduledCiteRuleDay Viernes { get; set; }
        public PatientId PatientId { get; set; }

        public ScheduledCiteRule(ScheduledCiteRuleId? id, ScheduledCiteRuleName name, ScheduledCiteRuleHour hour ,ScheduledCiteRuleStartDate startDate, ScheduledCiteRuleEndDate endDate, ScheduledCiteRuleDay lunes, ScheduledCiteRuleDay martes, ScheduledCiteRuleDay miercoles, ScheduledCiteRuleDay jueves, ScheduledCiteRuleDay viernes, PatientId patientId)
        {
            Id = id ?? new ScheduledCiteRuleId(0);
            Name = name;
            Hour = hour;
            StartDate = startDate;
            EndDate = endDate;
            Lunes = lunes;
            Martes = martes;
            Miercoles = miercoles;
            Jueves = jueves;
            Viernes = viernes;
            PatientId = patientId;
        }

        public static ScheduledCiteRule Create(ScheduledCiteRuleId? id, ScheduledCiteRuleName name, ScheduledCiteRuleHour hour, ScheduledCiteRuleStartDate startDate, ScheduledCiteRuleEndDate endDate, ScheduledCiteRuleDay lunes, ScheduledCiteRuleDay martes, ScheduledCiteRuleDay miercoles, ScheduledCiteRuleDay jueves, ScheduledCiteRuleDay viernes, PatientId patientId)
        {
            return new ScheduledCiteRule(id, name, hour, startDate, endDate, lunes, martes, miercoles, jueves, viernes, patientId);
        }

        public static ScheduledCiteRule FromPrimitives(int id, string name, TimeSpan hour, DateTime startDate, DateTime endDate, bool lunes, bool martes, bool miercoles, bool jueves, bool viernes, int patientId)
        {
            return new ScheduledCiteRule(
                new ScheduledCiteRuleId(id),
                new ScheduledCiteRuleName(name),
                new ScheduledCiteRuleHour(hour),
                new ScheduledCiteRuleStartDate(startDate),
                new ScheduledCiteRuleEndDate(endDate),
                new ScheduledCiteRuleDay(lunes),
                new ScheduledCiteRuleDay(martes),
                new ScheduledCiteRuleDay(miercoles),
                new ScheduledCiteRuleDay(jueves),
                new ScheduledCiteRuleDay(viernes),
                new PatientId(patientId)
            );
        }

        public void SetId(ScheduledCiteRuleId id)
        {
            if (Id.Value != 0)
            {
                throw new InvalidOperationException("El ID ya ha sido establecido.");
            }
            Id = id;
        }
    }
}
