using GestorEnfermeriaJoyfe.Domain.Cite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Infraestructure.CitePersistence
{
    public class CiteWithPatientInfoMapper : CiteMapper
    {
        public new Cite Map(IDataReader reader)
        {
            int id = reader.GetInt32(reader.GetOrdinal("id"));
            int patientId = reader.GetInt32(reader.GetOrdinal("patient_id"));
            string note = reader.GetString(reader.GetOrdinal("note"));
            int? visitId = reader.IsDBNull(reader.GetOrdinal("visit_id")) ? null : reader.GetInt32(reader.GetOrdinal("visit_id"));
            DateTime date = reader.GetDateTime(reader.GetOrdinal("date"));
            string? patientInfo = reader.IsDBNull(reader.GetOrdinal("patient_info")) ? null : reader.GetString(reader.GetOrdinal("patient_info"));

            return Cite.FromPrimitives(id, patientId, note, visitId, date, patientInfo);
        }
    }
}
