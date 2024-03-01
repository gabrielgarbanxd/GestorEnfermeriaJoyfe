using GestorEnfermeriaJoyfe.Domain.Cite.ValueObjects;
using GestorEnfermeriaJoyfe.Domain.Patient.ValueObjects;
using GestorEnfermeriaJoyfe.Domain.Shared;
using GestorEnfermeriaJoyfe.Domain.Visit.ValueObjects;
using System;

namespace GestorEnfermeriaJoyfe.Domain.Cite
{
    public class Cite
    {
        public CiteId Id { get; private set; }
        public PatientId PatientId { get; private set; }
        public CiteNote Note { get; set; }
        public VisitId VisitId { get; set; }
        public CiteDate Date { get; set; }

        public Cite(CiteId? id, PatientId patientId, CiteNote note, VisitId visitId, CiteDate date)
        {
            Id = id ?? new CiteId(0);
            PatientId = patientId;
            Note = note;
            VisitId = visitId;
            Date = Date;
        }

        public static Cite Create(CiteId? id, PatientId patientId, CiteNote note, VisitId visitId, CiteDate date)
        {
            return new Cite(id, patientId, note, visitId, date);
        }

        public static Cite FromPrimitives(int? id, int patientId, string note, int visitId, DateTime date)
        {
            return new Cite(new CiteId(id ?? 0), new PatientId(patientId), new CiteNote(note), new VisitId(visitId), new CiteDate(date));
        }

        public void SetId(CiteId id)
        {
            if (Id.Value != 0)
            {
                throw new InvalidOperationException("El ID ya ha sido establecido.");
            }
            Id = id;
        }
    }
}

