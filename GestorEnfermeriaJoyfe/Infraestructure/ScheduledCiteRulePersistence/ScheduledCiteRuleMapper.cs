using GestorEnfermeriaJoyfe.Domain.ScheduledCiteRule;
using GestorEnfermeriaJoyfe.Infraestructure.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Infraestructure.ScheduledCiteRulePersistence
{
    public class ScheduledCiteRuleMapper : IObjectMapper<ScheduledCiteRule>
    {
        public ScheduledCiteRule Map(IDataReader reader)
        {
            int id = reader.GetInt32(reader.GetOrdinal("id"));
            string name = reader.GetString(reader.GetOrdinal("name"));
            TimeSpan hour = (TimeSpan)reader.GetValue(reader.GetOrdinal("hour"));
            DateTime startDate = reader.GetDateTime(reader.GetOrdinal("start_date"));
            DateTime endDate = reader.GetDateTime(reader.GetOrdinal("end_date"));
            bool lunes = reader.GetBoolean(reader.GetOrdinal("lunes"));
            bool martes = reader.GetBoolean(reader.GetOrdinal("martes"));
            bool miercoles = reader.GetBoolean(reader.GetOrdinal("miercoles"));
            bool jueves = reader.GetBoolean(reader.GetOrdinal("jueves"));
            bool viernes = reader.GetBoolean(reader.GetOrdinal("viernes"));
            int patientId = reader.GetInt32(reader.GetOrdinal("patient_id"));

            return ScheduledCiteRule.FromPrimitives(id, name, hour, startDate, endDate, lunes, martes, miercoles, jueves, viernes, patientId);
            
        }
    }
}
