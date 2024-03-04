using GestorEnfermeriaJoyfe.Domain.Patient;
using System;

namespace GestorEnfermeriaJoyfe.UI.ViewModels
{
    public class MainViewModelRouter
    {
        public static readonly MainViewModelRouter Instance = new();
        private MainViewModelRouter() { }


        public event Action<string> ShowPrincipalView;

        public event Action<string> ShowPacientesView;

        public event Action<Patient> ShowSinglePacienteView;

        public event Action<Patient> ShowPacienteVisitsView;

        public event Action<Patient> ShowPacienteCitasView;

        public event Action<Patient> ShowCalendarView;

        public void OnShowPrincipalView(string view)
        {
            ShowPrincipalView?.Invoke(view);
        }

        public void OnShowPacientesView(string view)
        {
            ShowPacientesView?.Invoke(view);
        }

        public void OnShowSinglePacienteView(Patient patient)
        {
            ShowSinglePacienteView?.Invoke(patient);
        }

        public void OnShowPacienteVisitsView(Patient patient)
        {
            ShowPacienteVisitsView?.Invoke(patient);
        }

        public void OnShowCalendarView(Patient patient)
        {
            ShowCalendarView?.Invoke(patient);
        }


        public void OnShowCitasView(Patient patient)
        {
            ShowPacienteCitasView?.Invoke(patient);
        }


    }
}
