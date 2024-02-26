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
}
