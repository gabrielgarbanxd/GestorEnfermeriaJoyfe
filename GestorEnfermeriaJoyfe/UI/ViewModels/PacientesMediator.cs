using GestorEnfermeriaJoyfe.Domain.Patient;
using System;

namespace GestorEnfermeriaJoyfe.UI.ViewModels
{
    public class PacientesMediator
    {
        public static readonly PacientesMediator Instance = new();
        private PacientesMediator() { }

        public event Action<Patient> PacienteUpdated;
        public event Action<Patient> PacienteDeleted;
        public event Action<Patient> PacienteCreated;

        public void OnPacienteUpdated(Patient updatedPaciente)
        {
            PacienteUpdated?.Invoke(updatedPaciente);
        }

        public void OnPacienteDeleted(Patient deletedPaciente)
        {
            PacienteDeleted?.Invoke(deletedPaciente);
        }

        public void OnPacienteCreated(Patient newPaciente)
        {
            PacienteCreated?.Invoke(newPaciente);
        }
    }
}
