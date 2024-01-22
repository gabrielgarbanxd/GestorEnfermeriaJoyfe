using GestorEnfermeriaJoyfe.Domain;
using System;

namespace GestorEnfermeriaJoyfe.UI.ViewModels
{
    public class PacientesMediator
    {
        public static readonly PacientesMediator Instance = new PacientesMediator();
        private PacientesMediator() { }

        public event Action<PacienteModel> PacienteUpdated;
        public event Action<PacienteModel> PacienteDeleted;
        public event Action<PacienteModel> PacienteCreated;

        public void OnPacienteUpdated(PacienteModel updatedPaciente)
        {
            PacienteUpdated?.Invoke(updatedPaciente);
        }

        public void OnPacienteDeleted(PacienteModel deletedPaciente)
        {
            PacienteDeleted?.Invoke(deletedPaciente);
        }

        public void OnPacienteCreated(PacienteModel newPaciente)
        {
            PacienteCreated?.Invoke(newPaciente);
        }
    }
}
