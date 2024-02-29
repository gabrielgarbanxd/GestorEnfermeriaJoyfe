using GestorEnfermeriaJoyfe.Domain.Cites;
using GestorEnfermeriaJoyfe.Domain.Cites.ValueObjects;
using GestorEnfermeriaJoyfe.Infraestructure.Shared;
using System;
using System.Data;

namespace GestorEnfermeriaJoyfe.Infraestructure.CitesPersistence
{
    public class CitesMapper : IObjectMapper<Cite>
    {
        public Cite Map(IDataReader reader)
        {
            int id = reader.GetInt32(reader.GetOrdinal("id"));
            int patientId = reader.GetInt32(reader.GetOrdinal("patient_id"));
            string? note = reader.GetString(reader.GetOrdinal("nota"));
            int? visitId = reader.GetInt32(reader.GetOrdinal("visit_id"));
            DateTime date = reader.GetDateTime(reader.GetOrdinal("date"));

            return Cite.FromPrimitives(id, patientId, note, visitId, date);
        }
    }
}
