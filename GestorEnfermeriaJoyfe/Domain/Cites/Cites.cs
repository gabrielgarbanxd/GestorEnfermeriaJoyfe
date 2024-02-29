using GestorEnfermeriaJoyfe.Domain.Cites.ValueObjects;
using System;

namespace GestorEnfermeriaJoyfe.Domain.Cites
{
    public class Cite
    {
        public CitesId Id { get; private set; }
        public int PatientId { get; private set; }
        public string Note { get; private set; }
        public int? VisitId { get; private set; }
        public DateTime Date { get; private set; }

        public Cite(CitesId? id, int patientId, string note, int? visitId, DateTime date)
        {
            Id = id ?? new CitesId(0);
            PatientId = patientId;
            Note = note;
            VisitId = visitId;
            Date = date;
        }

        public static Cite Create(CitesId? id, int patientId, string note, int? visitId, DateTime date)
        {
            return new Cite(id, patientId, note, visitId, date);
        }

        public static Cite FromPrimitives(int? id, int patientId, string note, int? visitId, DateTime date)
        {
            return new Cite(new CitesId(id ?? 0), patientId, note, visitId, date);
        }
    }
}
