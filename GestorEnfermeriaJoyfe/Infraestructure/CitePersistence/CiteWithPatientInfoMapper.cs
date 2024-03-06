using GestorEnfermeriaJoyfe.Domain.Cite;
using System;
using System.Data;

namespace GestorEnfermeriaJoyfe.Infraestructure.CitePersistence
{
    public class CiteWithPatientInfoMapper : CiteMapper
    {
        public override Cite Map(IDataReader reader)
        {
            int id = reader.GetInt32(reader.GetOrdinal("id"));
            int patientId = reader.GetInt32(reader.GetOrdinal("patient_id"));
            string note = reader.GetString(reader.GetOrdinal("note"));
            int? visitId = reader.IsDBNull(reader.GetOrdinal("visit_id")) ? null : reader.GetInt32(reader.GetOrdinal("visit_id"));
            DateTime date = reader.GetDateTime(reader.GetOrdinal("date"));
            string? patientInfo;
            try
            {
                patientInfo = reader.IsDBNull(reader.GetOrdinal("patient_info")) ? null : reader.GetString(reader.GetOrdinal("patient_info"));
            }
            catch (Exception)
            {
                patientInfo = null;
            }

            return Cite.FromPrimitives(id, patientId, note, visitId, date, patientInfo);
        }
    }
}
