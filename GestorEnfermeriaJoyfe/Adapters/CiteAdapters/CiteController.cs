using GestorEnfermeriaJoyfe.Adapters.PatientAdapters;
using GestorEnfermeriaJoyfe.Domain.Cite;
using GestorEnfermeriaJoyfe.Infraestructure.CitePersistence;
using System;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Adapters.CiteAdapters
{
    public class CiteController
    {
        private readonly CiteMapper citeMapper;
        private readonly MySqlCiteRepository citeRepository;

        private readonly CiteQueryAdapter CiteQueryAdapter;
        private readonly CiteCommandAdapter CiteCommandAdapter;


        public CiteController()
        {
            citeMapper = new CiteWithPatientInfoMapper();
            citeRepository = new (citeMapper);


            CiteQueryAdapter = new(citeRepository, new());
            CiteCommandAdapter = new(citeRepository);
        }

        // ================== QUERYS ==================

        public async Task<CiteResponse> GetAll() => await CiteQueryAdapter.GetAllCites();
        public async Task<CiteResponse> GetAllPaginated(int perPage, int page) => await CiteQueryAdapter.GetAllCitesPaginated(perPage, page);
        public async Task<CiteResponse> Get(int id) => await CiteQueryAdapter.FindCite(id);

        public async Task<CiteResponse> SearchByPatientId(int patientId) => await CiteQueryAdapter.SearchByPatientId(patientId);
        public async Task<CiteResponse> SearchByPatientIdPaginated(int patientId, int perPage, int page) => await CiteQueryAdapter.SearchByPatientIdPaginated(patientId, perPage, page);

        public async Task<CiteResponse> SearchByDate(DateTime date) => await CiteQueryAdapter.SearchByDate(date);
        public async Task<CiteResponse> SearchByDatePaginated(DateTime date, int perPage, int page) => await CiteQueryAdapter.SearchByDatePaginated(date, perPage, page);

        public async Task<CiteResponse> SearchByDateRange(DateTime start, DateTime end) => await CiteQueryAdapter.SearchByDateRange(start, end);
        public async Task<CiteResponse> SearchByDateRangePaginated(DateTime start, DateTime end, int perPage, int page) => await CiteQueryAdapter.SearchByDateRangePaginated(start, end, perPage, page);


        public async Task<CiteResponse> SearchByPatientIdAndDate(int patientId, DateTime date) => await CiteQueryAdapter.SearchByPatientIdAndDate(patientId, date);
        public async Task<CiteResponse> SearchByPatientIdAndDatePaginated(int patientId, DateTime date, int perPage, int page) => await CiteQueryAdapter.SearchByPatientIdAndDatePaginated(patientId, date, perPage, page);

        public async Task<CiteResponse> SearchByPatientIdAndDateRange(int patientId, DateTime start, DateTime end) => await CiteQueryAdapter.SearchByPatientIdAndDateRange(patientId, start, end);
        public async Task<CiteResponse> SearchByPatientIdAndDateRangePaginated(int patientId, DateTime start, DateTime end, int perPage, int page) => await CiteQueryAdapter.SearchByPatientIdAndDateRangePaginated(patientId, start, end, perPage, page);


        public async Task<CiteResponse> GetAllWithPatientInfo() => await CiteQueryAdapter.GetAllCitesWithPatientInfo();
        public async Task<CiteResponse> GetAllWithPatientInfoPaginated(int perPage, int page) => await CiteQueryAdapter.GetAllCitesWithPatientInfoPaginated(perPage, page);


        public async Task<CiteResponse> SearchByDateWithPatientInfo(DateTime date) => await CiteQueryAdapter.SearchByDayWithPatientInfo(date);
        public async Task<CiteResponse> SearchByDateWithPatientInfoPaginated(DateTime date, int perPage, int page) => await CiteQueryAdapter.SearchByDayWithPatientInfoPaginated(date, perPage, page);



        // ================== COMMANDS ==================
        public async Task<CommandResponse> Create(Cite cite) => await CiteCommandAdapter.Create(cite);
        public async Task<CommandResponse> Update(Cite cite) => await CiteCommandAdapter.Update(cite);
        public async Task<CommandResponse> Delete(int id) => await CiteCommandAdapter.Delete(id);
    }
}
