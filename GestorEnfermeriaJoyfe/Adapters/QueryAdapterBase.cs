using GestorEnfermeriaJoyfe.Adapters.VisitAdapters;
using GestorEnfermeriaJoyfe.ApplicationLayer.PatientApp;
using GestorEnfermeriaJoyfe.Domain.Patient;
using GestorEnfermeriaJoyfe.Domain.Visit;
using GestorEnfermeriaJoyfe.Infraestructure.PatientPersistence;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

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
                MessageBox.Show(ex.Message);
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

        public override PatientResponse Ok(string? message = null, IEnumerable<Patient>? data = default)
        {
            return new PatientResponse(true, message, data);
        }

        public override PatientResponse Fail(string? message = null)
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

    public class PatientController2
    {
        private readonly PatientMapper patientMapper;
        private readonly MySqlPatientRepository patientRepository;

        private readonly QueryAdapter queryAdapter;

        public PatientController2()
        {
            patientMapper = new();
            patientRepository = new(patientMapper);

            queryAdapter = new(new PatientResponse(), patientRepository);
        }

        public async Task<PatientResponse> GetAll() => await queryAdapter.GetAll();
    }

}
