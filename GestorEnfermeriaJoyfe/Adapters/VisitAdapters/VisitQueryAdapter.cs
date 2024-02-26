using GestorEnfermeriaJoyfe.ApplicationLayer.VisitApp;
using GestorEnfermeriaJoyfe.ApplicationLayer.VisitApp.SearchBy;
using GestorEnfermeriaJoyfe.ApplicationLayer.VisitApp.SearchBy.Mixed;
using GestorEnfermeriaJoyfe.Domain.Visit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Adapters.VisitAdapters
{
    public class VisitQueryAdapter : QueryAdapterBase<VisitResponse, IEnumerable<Visit>>
    {
        private readonly IVisitContract visitRepository;

        public VisitQueryAdapter(IVisitContract visitRepository, VisitResponse response) : base(response)
        {
            this.visitRepository = visitRepository;
        }

        public async Task<VisitResponse> FindVisit(int id)
        {
            return await RunQuery(async () =>
            {
                return new List<Visit> { await new VisitFinder(visitRepository).Run(id) };
            });
        }

        public async Task<VisitResponse> GetAllVisits()
        {
            return await RunQuery(async () =>
            {
                return await new VisitLister(visitRepository).Run();
            });
        }

        public async Task<VisitResponse> GetAllVisitsPaginated(int perPage, int page)
        {
            return await RunQuery(async () =>
            {
                return await new VisitLister(visitRepository).Run(true, perPage, page);
            });
        }

        public async Task<VisitResponse> SearchByPatientId(int patientId)
        {
            return await RunQuery(async () =>
            {
                return await new VisitsByPatientIdSearcher(visitRepository).Run(patientId);
            });
        }

        public async Task<VisitResponse> SearchByPatientIdPaginated(int patientId, int perPage, int page)
        {
            return await RunQuery(async () =>
            {
                return await new VisitsByPatientIdSearcher(visitRepository).Run(patientId, true, perPage, page);
            });
        }

        public async Task<VisitResponse> SearchByDate(DateTime date)
        {
            return await RunQuery(async () =>
            {
                return await new VisitsByDateSearcher(visitRepository).Run(date);
            });
        }

        public async Task<VisitResponse> SearchByDatePaginated(DateTime date, int perPage, int page)
        {
            return await RunQuery(async () =>
            {
                return await new VisitsByDateSearcher(visitRepository).Run(date, true, perPage, page);
            });
        }
        
        public async Task<VisitResponse> SearchByDateRange(DateTime startDate, DateTime endDate)
        {
            return await RunQuery(async () =>
            {
                return await new VisitsByDateRangeSearcher(visitRepository).Run(startDate, endDate);
            });
        }

        public async Task<VisitResponse> SearchByDateRangePaginated(DateTime startDate, DateTime endDate, int perPage, int page)
        {
            return await RunQuery(async () =>
            {
                return await new VisitsByDateRangeSearcher(visitRepository).Run(startDate, endDate, true, perPage, page);
            });
        }

        public async Task<VisitResponse> SearchByPatientIdAndDate(int patientId, DateTime date)
        {
            return await RunQuery(async () =>
            {
                return await new VisitsByPatientIdAndDateSearcher(visitRepository).Run(patientId, date);
            });
        }

        public async Task<VisitResponse> SearchByPatientIdAndDatePaginated(int patientId, DateTime date, int perPage, int page)
        {
            return await RunQuery(async () =>
            {
                return await new VisitsByPatientIdAndDateSearcher(visitRepository).Run(patientId, date, true, perPage, page);
            });
        }

        public async Task<VisitResponse> SearchByPatientIdAndDateRange(int patientId, DateTime startDate, DateTime endDate)
        {
            return await RunQuery(async () =>
            {
                return await new VisitsByPatientIdAndDateRangeSearcher(visitRepository).Run(patientId, startDate, endDate);
            });
        }

        public async Task<VisitResponse> SearchByPatientIdAndDateRangePaginated(int patientId, DateTime startDate, DateTime endDate, int perPage, int page)
        {
            return await RunQuery(async () =>
            {
                return await new VisitsByPatientIdAndDateRangeSearcher(visitRepository).Run(patientId, startDate, endDate, true, perPage, page);
            });
        }
    }
}
