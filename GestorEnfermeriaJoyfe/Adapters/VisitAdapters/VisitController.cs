using GestorEnfermeriaJoyfe.Domain.Visit;
using GestorEnfermeriaJoyfe.Infraestructure.VisitPersistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Adapters.VisitAdapters
{
    public class VisitController
    {
        private readonly VisitMapper visitMapper;
        private readonly MySqlVisitRepository visitRepository;

        private readonly VisitQueryAdapter visitQueryAdapter;
        private readonly VisitCommandAdapter visitCommandAdapter;

        public VisitController()
        {
            visitMapper = new VisitWithPatientInfoMapper();
            visitRepository = new(visitMapper);

            visitQueryAdapter = new(visitRepository, new());
            visitCommandAdapter = new(visitRepository);
        }

        // ================== QUERYS ==================

        public async Task<VisitResponse> GerAll() => await visitQueryAdapter.GetAllVisits();
        public async Task<VisitResponse> GetAllPaginated(int perPage, int page) => await visitQueryAdapter.GetAllVisitsPaginated(perPage, page);
        public async Task<VisitResponse> Get(int id) => await visitQueryAdapter.FindVisit(id);
        public async Task<VisitResponse> SearchByPatientId(int patientId) => await visitQueryAdapter.SearchByPatientId(patientId);
        public async Task<VisitResponse> SearchByPatientIdPaginated(int patientId, int perPage, int page) => await visitQueryAdapter.SearchByPatientIdPaginated(patientId, perPage, page);
        public async Task<VisitResponse> SearchByDate(DateTime date) => await visitQueryAdapter.SearchByDate(date);
        public async Task<VisitResponse> SearchByDatePaginated(DateTime date, int perPage, int page) => await visitQueryAdapter.SearchByDatePaginated(date, perPage, page);
        public async Task<VisitResponse> SearchByDateRange(DateTime start, DateTime end) => await visitQueryAdapter.SearchByDateRange(start, end);
        public async Task<VisitResponse> SearchByDateRangePaginated(DateTime start, DateTime end, int perPage, int page) => await visitQueryAdapter.SearchByDateRangePaginated(start, end, perPage, page);
        public async Task<VisitResponse> SearchByPatientIdAndDate(int patientId, DateTime date) => await visitQueryAdapter.SearchByPatientIdAndDate(patientId, date);
        public async Task<VisitResponse> SearchByPatientIdAndDatePaginated(int patientId, DateTime date, int perPage, int page) => await visitQueryAdapter.SearchByPatientIdAndDatePaginated(patientId, date, perPage, page);
        public async Task<VisitResponse> SearchByPatientIdAndDateRange(int patientId, DateTime start, DateTime end) => await visitQueryAdapter.SearchByPatientIdAndDateRange(patientId, start, end);
        public async Task<VisitResponse> SearchByPatientIdAndDateRangePaginated(int patientId, DateTime start, DateTime end, int perPage, int page) => await visitQueryAdapter.SearchByPatientIdAndDateRangePaginated(patientId, start, end, perPage, page);
        public async Task<VisitResponse> SearchByDateWithPatientInfo(DateTime date) => await visitQueryAdapter.SearchByDateWithPatientInfo(date);
        public async Task<VisitResponse> SearchByDateWithPatientInfoPaginated(DateTime date, int perPage, int page) => await visitQueryAdapter.SearchByDateWithPatientInfoPaginated(date, perPage, page);

        // ================== COMMANDS ==================

        public async Task<CommandResponse> Register(Visit visit) => await visitCommandAdapter.CreateVisit(visit);
        public async Task<CommandResponse> Update(Visit visit) => await visitCommandAdapter.UpdateVisit(visit);
        public async Task<CommandResponse> Delete(int id) => await visitCommandAdapter.DeleteVisit(id);
    }
}
