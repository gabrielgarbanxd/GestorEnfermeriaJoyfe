using GestorEnfermeriaJoyfe.Adapters.VisitAdapters;
using GestorEnfermeriaJoyfe.ApplicationLayer.PatientApp;
using GestorEnfermeriaJoyfe.Domain.Patient;
using GestorEnfermeriaJoyfe.Domain.Visit;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Adapters
{
    public abstract class QueryAdapterBase<TResponse, T> where TResponse : ResponseBase<T>
    {
        public TResponse QueryResponse { get; private set; }

        public QueryAdapterBase(TResponse response)
        {
            QueryResponse = response;
        }

        public async Task<TResponse> RunQuery(Func<Task<T>> query, string? message = null)
        {
            try
            {
                return (TResponse)QueryResponse.Ok(message, await query());
            }
            catch (Exception ex)
            {
                return (TResponse)QueryResponse.Fail(ex.Message);
            }
        }
    }

    public class PatientResponse : ResponseBase<IEnumerable<Patient>>
    {
        public PatientResponse(bool success, string? message, IEnumerable<Patient>? data) : base(success, message, data)
        {
        }

        public PatientResponse(bool success, string? message) : base(success, message)
        {
        }

        public PatientResponse()
        {
        }

        public static new PatientResponse Ok(string? message = null, IEnumerable<Patient>? data = default)
        {
            return new PatientResponse(true, message, data);
        }

        public static new PatientResponse Fail(string? message = null)
        {
            return new PatientResponse(false, message);
        }
    }

    public class QueryAdapter : QueryAdapterBase<PatientResponse, IEnumerable<Patient>>
    {
        private readonly IPatientContract patientRepository;

        public QueryAdapter(PatientResponse response, IPatientContract patientRepository) : base(response)
        {
            this.patientRepository = patientRepository;
        }

        public async Task<PatientResponse> GetAll()
        {
            return await RunQuery(async () =>
            {
                return await new PatientLister(patientRepository).Run();
            });

        }

}
