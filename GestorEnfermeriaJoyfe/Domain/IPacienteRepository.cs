using System.Collections.Generic;

namespace GestorEnfermeriaJoyfe.Domain
{
    public interface IPacienteRepository
    {
        List<PacienteModel> All();
        PacienteModel GetById(int id);
        PacienteModel Create(PacienteModel model);
        bool Update(PacienteModel model);
        bool Delete(int id);
    }
}
