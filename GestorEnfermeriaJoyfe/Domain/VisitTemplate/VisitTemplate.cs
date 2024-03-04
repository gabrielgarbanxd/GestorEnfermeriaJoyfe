using GestorEnfermeriaJoyfe.Domain.Visit.ValueObjects;
using GestorEnfermeriaJoyfe.Domain.VisitTemplate.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Domain.VisitTemplate
{
    public class VisitTemplate
    {
        public VisitTemplateId Id { get; private set; }
        public VisitTemplateName Name { get; set; }
        public VisitType Type { get; set; }
        public VisitClassification Classification { get; set; }
        public VisitIsComunicated IsComunicated { get; set; }
        public VisitIsDerived IsDerived { get; set; }
        public VisitTraumaType? TraumaType { get; set; }
        public VisitPlace? Place { get; set; }

        public VisitTemplate(VisitTemplateId? id, VisitTemplateName name, VisitType type, VisitClassification classification, VisitIsComunicated isComunicated, VisitIsDerived isDerived, VisitTraumaType? traumaType, VisitPlace? place)
        {
            Id = id ?? new VisitTemplateId(0);
            Name = name;
            Type = type;
            Classification = classification;
            IsComunicated = isComunicated;
            IsDerived = isDerived;

            EnsureTraumaTypeAndPlaceNotNullIfNotDerived(isDerived.Value, traumaType, place);

            TraumaType = isDerived.Value ? null : traumaType; // Si IsDerived es verdadero, asigna null a TraumaType
            Place = isDerived.Value ? null : place; // Si IsDerived es verdadero, asigna null a Place
        }

        public static VisitTemplate Create(VisitTemplateId? id, VisitTemplateName name, VisitType type, VisitClassification classification, VisitIsComunicated isComunicated, VisitIsDerived isDerived, VisitTraumaType? traumaType, VisitPlace? place)
        {
            return new VisitTemplate(id, name, type, classification, isComunicated, isDerived, traumaType, place);
        }

        public static VisitTemplate FromPrimitives(int id, string name, string type, string classification, bool isComunicated, bool isDerived, string? traumaType, string? place)
        {
            return new VisitTemplate(
                new VisitTemplateId(id),
                new VisitTemplateName(name),
                new VisitType(type),
                new VisitClassification(classification),
                new VisitIsComunicated(isComunicated),
                new VisitIsDerived(isDerived),
                traumaType != null ? new VisitTraumaType(traumaType) : null,
                place != null ? new VisitPlace(place) : null
            );
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
