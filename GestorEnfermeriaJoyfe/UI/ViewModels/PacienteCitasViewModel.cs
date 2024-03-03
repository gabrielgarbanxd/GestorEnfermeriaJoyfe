using GestorEnfermeriaJoyfe.Domain.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.UI.ViewModels
{
    public class PacienteCitasViewModel : ViewModelBase
    {
        private readonly Patient patient;

        public PacienteCitasViewModel(Patient patient)
        {
            this.patient = patient;
        }
    }
}
