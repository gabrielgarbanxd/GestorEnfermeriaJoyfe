using GestorEnfermeriaJoyfe.ApplicationLayer.VisitApp;
using GestorEnfermeriaJoyfe.ApplicationLayer.VisitApp.SearchBy;
using GestorEnfermeriaJoyfe.ApplicationLayer.VisitApp.SearchBy.Mixed;
using GestorEnfermeriaJoyfe.Domain.Visit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Adapters.VisitAdapters
{
    public class VisitQueryAdapter
    {
        private readonly IVisitContract visitRepository;

        public VisitQueryAdapter(IVisitContract visitRepository) => this.visitRepository = visitRepository;

        public async Task<Response<Visit>> FindVisit(int id)
        {
            try
            {
                VisitFinder visitFinder = new(visitRepository);

                var visit = await visitFinder.Run(id);

                return Response<Visit>.Ok("Visita encontrada", visit);
            }
            catch (Exception e)
            {
                return Response<Visit>.Fail(e.Message);
            }
        }

        public async Task<Response<IEnumerable<Visit>>> GetAllVisits()
        {
            try
            {
                VisitLister visitLister = new(visitRepository);

                var visits = await visitLister.Run();

                return Response<IEnumerable<Visit>>.Ok("Visitas encontradas", visits);
            }
            catch (Exception e)
            {
                return Response<IEnumerable<Visit>>.Fail(e.Message);
            }
        }

        public async Task<Response<IEnumerable<Visit>>> GetAllVisitsPaginated(int perPage, int page)
        {
            try
            {
                VisitLister visitLister = new(visitRepository);

                var visits = await visitLister.Run(true, perPage, page);

                return Response<IEnumerable<Visit>>.Ok("Visitas encontradas", visits);
            }
            catch (Exception e)
            {
                return Response<IEnumerable<Visit>>.Fail(e.Message);
            }
        }

        public async Task<Response<IEnumerable<Visit>>> SearchByPatientId (int patientId)
        {
            try
            {
                VisitsByPatientIdSearcher visitsByPatientIdSearcher = new(visitRepository);

                var visits = await visitsByPatientIdSearcher.Run(patientId);

                return Response<IEnumerable<Visit>>.Ok("Visitas encontradas", visits);
            }
            catch (Exception e)
            {
                return Response<IEnumerable<Visit>>.Fail(e.Message);
            }
        }

        public async Task<Response<IEnumerable<Visit>>> SearchByPatientIdPaginated(int patientId, int perPage, int page)
        {
            try
            {
                VisitsByPatientIdSearcher visitsByPatientIdSearcher = new(visitRepository);

                var visits = await visitsByPatientIdSearcher.Run(patientId, true, perPage, page);

                return Response<IEnumerable<Visit>>.Ok("Visitas encontradas", visits);
            }
            catch (Exception e)
            {
                return Response<IEnumerable<Visit>>.Fail(e.Message);
            }
        }


        public async Task<Response<IEnumerable<Visit>>> SearchByDate(DateTime date)
        {
            try
            {
                VisitsByDateSearcher visitsByDateSearcher = new(visitRepository);

                var visits = await visitsByDateSearcher.Run(date);

                return Response<IEnumerable<Visit>>.Ok("Visitas encontradas", visits);
            }
            catch (Exception e)
            {
                return Response<IEnumerable<Visit>>.Fail(e.Message);
            }
        }

        public async Task<Response<IEnumerable<Visit>>> SearchByDatePaginated(DateTime date, int perPage, int page)
        {
            try
            {
                VisitsByDateSearcher visitsByDateSearcher = new(visitRepository);

                var visits = await visitsByDateSearcher.Run(date, true, perPage, page);

                return Response<IEnumerable<Visit>>.Ok("Visitas encontradas", visits);
            }
            catch (Exception e)
            {
                return Response<IEnumerable<Visit>>.Fail(e.Message);
            }
        }

        public async Task<Response<IEnumerable<Visit>>> SearchByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                VisitsByDateRangeSearcher visitsByDateRangeSearcher = new(visitRepository);

                var visits = await visitsByDateRangeSearcher.Run(startDate, endDate);

                return Response<IEnumerable<Visit>>.Ok("Visitas encontradas", visits);
            }
            catch (Exception e)
            {
                return Response<IEnumerable<Visit>>.Fail(e.Message);
            }
        }

        public async Task<Response<IEnumerable<Visit>>> SearchByDateRangePaginated(DateTime startDate, DateTime endDate, int perPage, int page)
        {
            try
            {
                VisitsByDateRangeSearcher visitsByDateRangeSearcher = new(visitRepository);

                var visits = await visitsByDateRangeSearcher.Run(startDate, endDate, true, perPage, page);

                return Response<IEnumerable<Visit>>.Ok("Visitas encontradas", visits);
            }
            catch (Exception e)
            {
                return Response<IEnumerable<Visit>>.Fail(e.Message);
            }
        }

        public async Task<Response<IEnumerable<Visit>>> SearchByPatientIdAndDate(int patientId, DateTime date)
        {
            try
            {
                VisitsByPatientIdAndDateSearcher visitsByPatientIdAndDateSearcher = new(visitRepository);

                var visits = await visitsByPatientIdAndDateSearcher.Run(patientId, date);

                return Response<IEnumerable<Visit>>.Ok("Visitas encontradas", visits);
            }
            catch (Exception e)
            {
                return Response<IEnumerable<Visit>>.Fail(e.Message);
            }
        }

        public async Task<Response<IEnumerable<Visit>>> SearchByPatientIdAndDatePaginated(int patientId, DateTime date, int perPage, int page)
        {
            try
            {
                VisitsByPatientIdAndDateSearcher visitsByPatientIdAndDateSearcher = new(visitRepository);

                var visits = await visitsByPatientIdAndDateSearcher.Run(patientId, date, true, perPage, page);

                return Response<IEnumerable<Visit>>.Ok("Visitas encontradas", visits);
            }
            catch (Exception e)
            {
                return Response<IEnumerable<Visit>>.Fail(e.Message);
            }
        }

        public async Task<Response<IEnumerable<Visit>>> SearchByPatientIdAndDateRange(int patientId, DateTime startDate, DateTime endDate)
        {
            try
            {
                VisitsByPatientIdAndDateRangeSearcher visitsByPatientIdAndDateRangeSearcher = new(visitRepository);

                var visits = await visitsByPatientIdAndDateRangeSearcher.Run(patientId, startDate, endDate);

                return Response<IEnumerable<Visit>>.Ok("Visitas encontradas", visits);
            }
            catch (Exception e)
            {
                return Response<IEnumerable<Visit>>.Fail(e.Message);
            }
        }

        public async Task<Response<IEnumerable<Visit>>> SearchByPatientIdAndDateRangePaginated(int patientId, DateTime startDate, DateTime endDate, int perPage, int page)
        {
            try
            {
                VisitsByPatientIdAndDateRangeSearcher visitsByPatientIdAndDateRangeSearcher = new(visitRepository);

                var visits = await visitsByPatientIdAndDateRangeSearcher.Run(patientId, startDate, endDate, true, perPage, page);

                return Response<IEnumerable<Visit>>.Ok("Visitas encontradas", visits);
            }
            catch (Exception e)
            {
                return Response<IEnumerable<Visit>>.Fail(e.Message);
            }
        }

    }
}
