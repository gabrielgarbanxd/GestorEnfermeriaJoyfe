using GestorEnfermeriaJoyfe.Domain.Patient.ValueObjects;
using GestorEnfermeriaJoyfe.Domain.Visit.ValueObjects;
using System;

namespace GestorEnfermeriaJoyfe.Domain.Visit
{
    public class Visit
    {
        public VisitId Id { get; private set; }
        public VisitType Type { get; set; }
        public VisitClassification Classification { get; set; }
        public VisitDescription? Description { get; set; }
        public VisitIsComunicated IsComunicated { get; set; }
        public VisitIsDerived IsDerived { get; set; }
        public VisitTraumaType? TraumaType { get; set; }
        public VisitPlace? Place { get; set; }
        public VisitDate Date { get; set; }
        public PatientId PatientId { get; set; }
        public VisitPatientInfo? PatientInfo { get; set; }

        public Visit(VisitId? id, VisitType type, VisitClassification classification, VisitDescription? description, VisitIsComunicated isComunicated, VisitIsDerived isDerived, VisitTraumaType? traumaType, VisitPlace? place, VisitDate date, PatientId patientId)
        {
            Id = id ?? new VisitId(0);
            Type = type;
            Classification = classification;
            Description = description;
            IsComunicated = isComunicated;
            IsDerived = isDerived;

            EnsureTraumaTypeAndPlaceNotNullIfNotDerived(isDerived.Value, traumaType, place);

            TraumaType = isDerived.Value ? null : traumaType; // Si IsDerived es verdadero, asigna null a TraumaType
            Place = isDerived.Value ? null : place; // Si IsDerived es verdadero, asigna null a Place
            Date = date;
            PatientId = patientId;
        }

        public static Visit Create(VisitId? id, VisitType type, VisitClassification classification, VisitDescription? description, VisitIsComunicated isComunicated, VisitIsDerived isDerived, VisitTraumaType? traumaType, VisitPlace? place, VisitDate date, PatientId patientId)
        {
            return new Visit(id, type, classification, description, isComunicated, isDerived, traumaType, place, date, patientId);
        }

        public static Visit FromPrimitives(int id, string type, string classification, string? description, bool isComunicated, bool isDerived, string? traumaType, string? place, DateTime date, int patientId)
        {
            return new Visit(
                new VisitId(id),
                new VisitType(type),
                new VisitClassification(classification),
                description != null ? new VisitDescription(description) : null,
                new VisitIsComunicated(isComunicated),
                new VisitIsDerived(isDerived),
                traumaType != null ? new VisitTraumaType(traumaType) : null,
                place != null ? new VisitPlace(place) : null,
                new VisitDate(date),
                new PatientId(patientId)
            );
        }

        public static Visit FromPrimitives(int id, string type, string classification, string? description, bool isComunicated, bool isDerived, string? traumaType, string? place, DateTime date, int patientId, string? patientInfo)
        {
            return new Visit(
                new VisitId(id),
                new VisitType(type),
                new VisitClassification(classification),
                description != null ? new VisitDescription(description) : null,
                new VisitIsComunicated(isComunicated),
                new VisitIsDerived(isDerived),
                traumaType != null ? new VisitTraumaType(traumaType) : null,
                place != null ? new VisitPlace(place) : null,
                new VisitDate(date),
                new PatientId(patientId))
            {
                PatientInfo = patientInfo != null ? new VisitPatientInfo(patientInfo) : null
            };
        }


        public void SetId(VisitId id)
        {
            if (Id.Value != 0)
            {
                throw new InvalidOperationException("El ID ya ha sido establecido.");
            }
            Id = id;
        }

        private void EnsureTraumaTypeAndPlaceNotNullIfNotDerived(bool isDerived, VisitTraumaType? traumaType, VisitPlace? place)
        {
            if (!isDerived && (traumaType == null || place == null))
            {
                throw new InvalidOperationException("El tipo de trauma y el lugar de la visita no pueden ser nulos si la visita no es derivada.");
            }
        }
    }
}
