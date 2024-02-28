using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.UI.ViewModels
{
    public class MainViewModelRouter
    {
        public static readonly MainViewModelRouter Instance = new();
        private MainViewModelRouter() { }


        public event Action<string> ShowPrincipalView;
        public event Action<string> ShowPacientesView;
        public event Action<int> ShowPacienteVisitsView;

        public event Action<string> ShowCalendarView;

        //public event Action<string> ShowConfiguracionView;
        //public event Action<string> ShowCitasView;

        public void OnShowPrincipalView(string view)
        {
            ShowPrincipalView?.Invoke(view);
        }

        public void OnShowPacientesView(string view)
        {
            ShowPacientesView?.Invoke(view);
        }

        public void OnShowPacienteVisitsView(int id)
        {
            ShowPacienteVisitsView?.Invoke(id);
        }

        public void OnShowCalendarView(string view)
        {
            ShowCalendarView?.Invoke(view);
        }

        //public void OnShowConfiguracionView(string view)
        //{
        //    ShowConfiguracionView?.Invoke(view);
        //}

        //public void OnShowCitasView(string view)
        //{
        //    ShowCitasView?.Invoke(view);
        //}


    }
}
