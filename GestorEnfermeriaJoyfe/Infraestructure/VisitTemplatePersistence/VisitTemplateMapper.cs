using GestorEnfermeriaJoyfe.Domain.VisitTemplate;
using GestorEnfermeriaJoyfe.Infraestructure.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Infraestructure.VisitTemplatePersistence
{
    public class VisitTemplateMapper : IObjectMapper<VisitTemplate>
    {
        public VisitTemplate Map(IDataReader reader)
        {
            int id = reader.GetInt32(reader.GetOrdinal("id"));
            string name = reader.GetString(reader.GetOrdinal("name"));
            string type = reader.GetString(reader.GetOrdinal("type"));
            string classification = reader.GetString(reader.GetOrdinal("classification"));
            int isComunicated = reader.GetInt32(reader.GetOrdinal("is_comunicated"));
            int isDerived = reader.GetInt32(reader.GetOrdinal("is_derived"));
            string? traumaType = !reader.IsDBNull(reader.GetOrdinal("trauma_type")) ? reader.GetString(reader.GetOrdinal("trauma_type")) : null;
            string? place = !reader.IsDBNull(reader.GetOrdinal("place")) ? reader.GetString(reader.GetOrdinal("place")) : null;

            return VisitTemplate.FromPrimitives(id, name, type, classification, isComunicated == 1, isDerived == 1, traumaType, place);
        }
    }
}
