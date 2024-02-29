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
            int patientId = reader.GetInt32(reader.GetOrdinal("id_paciente"));
            string note = reader.IsDBNull(reader.GetOrdinal("nota")) ? string.Empty : reader.GetString(reader.GetOrdinal("nota"));
            int? visitId = reader.IsDBNull(reader.GetOrdinal("id_visita")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("id_visita"));
            DateTime date = reader.GetDateTime(reader.GetOrdinal("fecha_cita"));

            return Cite.FromPrimitives(id, patientId, note, visitId, date);
        }
    }
}
