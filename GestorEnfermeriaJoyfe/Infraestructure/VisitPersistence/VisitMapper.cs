﻿using GestorEnfermeriaJoyfe.Domain.Patient.ValueObjects;
using GestorEnfermeriaJoyfe.Domain.Visit;
using GestorEnfermeriaJoyfe.Domain.Visit.ValueObjects;
using GestorEnfermeriaJoyfe.Infraestructure.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Infraestructure.VisitPersistence
{
    public class VisitMapper : IObjectMapper<Visit>
    {
        public Visit Map(IDataReader reader)
        {
            int id = reader.GetInt32(reader.GetOrdinal("id"));
            string type = reader.GetString(reader.GetOrdinal("type"));
            string classification = reader.GetString(reader.GetOrdinal("classification"));
            int isComunicated = reader.GetInt32(reader.GetOrdinal("is_comunicated"));
            int isDerived = reader.GetInt32(reader.GetOrdinal("is_derived"));
            string? traumaType = reader.GetString(reader.GetOrdinal("trauma_type"));
            string? place =reader.GetString(reader.GetOrdinal("place"));
            DateTime date = reader.GetDateTime(reader.GetOrdinal("date"));
            int patientId = reader.GetInt32(reader.GetOrdinal("patient_id"));

            return Visit.FromPrimitives(id, type,classification,isComunicated == 1,isDerived == 1, traumaType,place,date,patientId);
        }
    }
}
