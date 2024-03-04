using GestorEnfermeriaJoyfe.ApplicationLayer.CiteApp;
using GestorEnfermeriaJoyfe.Adapters.CiteAdapters;
using GestorEnfermeriaJoyfe.Domain.Cite;
using System.Collections.Generic;
using System.Threading.Tasks;
using GestorEnfermeriaJoyfe.ApplicationLayer.CiteApp.SearchBy;
using System;
using GestorEnfermeriaJoyfe.ApplicationLayer.CiteApp.SearchBy.Mixed;
using GestorEnfermeriaJoyfe.ApplicationLayer.CiteApp.WithPatientInfo;



namespace GestorEnfermeriaJoyfe.Adapters.CiteAdapters
{
    public class CiteQueryAdapter : QueryAdapterBase<CiteResponse, IEnumerable<Cite>>
    {
        private readonly ICiteContract citeRepository;

        public CiteQueryAdapter(ICiteContract citeRepository, CiteResponse response) : base(response)
        {
            this.citeRepository = citeRepository;
        }

        public async Task<CiteResponse> FindCite(int id)
        {
            return await RunQuery(async () =>
            {
                return new List<Cite> { await new CiteFinder(citeRepository).Run(id) };
            });
        }

        public async Task<CiteResponse> GetAllCites()
        {
            return await RunQuery(async () =>
            {
                return await new CiteLister(citeRepository).Run();
            });
        }

        public async Task<CiteResponse> GetAllCitesPaginated(int perPage, int page)
        {
            return await RunQuery(async () =>
            {
                return await new CiteLister(citeRepository).Run(true, perPage, page);
            });
        }

        // ======>> SEARCHERS <<=======

        // *** SEARCH BY PATIENT ID ***
        public async Task<CiteResponse> SearchByPatientId(int patientId)
        {
            return await RunQuery(async () =>
            {
                return await new CitesByPatientIdSearcher(citeRepository).Run(patientId);
            });
        }

        public async Task<CiteResponse> SearchByPatientIdPaginated(int patientId, int perPage, int page)
        {
            return await RunQuery(async () =>
            {
                return await new CitesByPatientIdSearcher(citeRepository).Run(patientId, true, perPage, page);
            });
        }

        // *** SEARCH BY DATE ***

        public async Task<CiteResponse> SearchByDate(DateTime date)
        {
            return await RunQuery(async () =>
            {
                return await new CitesByDaySearcher(citeRepository).Run(date);
            });
        }

        public async Task<CiteResponse> SearchByDatePaginated(DateTime date, int perPage, int page)
        {
            return await RunQuery(async () =>
            {
                return await new CitesByDaySearcher(citeRepository).Run(date, true, perPage, page);
            });
        }

        // *** SEARCH BY DATE RANGE ***
        
        public async Task<CiteResponse> SearchByDateRange(DateTime start, DateTime end)
        {
            return await RunQuery(async () =>
            {
                return await new CitesByDayRangeSearcher(citeRepository).Run(start, end);
            });
        }

        public async Task<CiteResponse> SearchByDateRangePaginated(DateTime start, DateTime end, int perPage, int page)
        {
            return await RunQuery(async () =>
            {
                return await new CitesByDayRangeSearcher(citeRepository).Run(start, end, true, perPage, page);
            });
        }


        // *** MIXED SEARCHERS ***

        public async Task<CiteResponse> SearchByPatientIdAndDate(int patientId, DateTime date)
        {
            return await RunQuery(async () =>
            {
                return await new CitesByDayAndPatientIdSearcher(citeRepository).Run(date, patientId);
            });
        }

        public async Task<CiteResponse> SearchByPatientIdAndDatePaginated(int patientId, DateTime date, int perPage, int page)
        {
            return await RunQuery(async () =>
            {
                return await new CitesByDayAndPatientIdSearcher(citeRepository).Run(date, patientId, true, perPage, page);
            });
        }

        public async Task<CiteResponse> SearchByPatientIdAndDateRange(int patientId, DateTime start, DateTime end)
        {
            return await RunQuery(async () =>
            {
                return await new CitesByPatientIdAndDayRangeSearcher(citeRepository).Run(patientId, start, end);
            });
        }

        public async Task<CiteResponse> SearchByPatientIdAndDateRangePaginated(int patientId, DateTime start, DateTime end, int perPage, int page)
        {
            return await RunQuery(async () =>
            {
                return await new CitesByPatientIdAndDayRangeSearcher(citeRepository).Run(patientId, start, end, true, perPage, page);
            });
        }


        // =====>> WITH PATIENT INFO <<=====

        public async Task<CiteResponse> GetAllCitesWithPatientInfo()
        {
            return await RunQuery(async () =>
            {
                return await new CiteWithPatientInfoLister(citeRepository).Run();
            });
        }

        public async Task<CiteResponse> GetAllCitesWithPatientInfoPaginated(int perPage, int page)
        {
            return await RunQuery(async () =>
            {
                return await new CiteWithPatientInfoLister(citeRepository).Run(true, perPage, page);
            });
        }


        public async Task<CiteResponse> SearchByDayWithPatientInfo(DateTime date)
        {
            return await RunQuery(async () =>
            {
                return await new CitesByDayWithPatientInfoSearcher(citeRepository).Run(date);
            });
        }

        public async Task<CiteResponse> SearchByDayWithPatientInfoPaginated(DateTime date, int perPage, int page)
        {
            return await RunQuery(async () =>
            {
                return await new CitesByDayWithPatientInfoSearcher(citeRepository).Run(date, true, perPage, page);
            });
        }
    }
}

