using GestorEnfermeriaJoyfe.Domain.Patient;
using GestorEnfermeriaJoyfe.Domain.Patient.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.ApplicationLayer.PatientApp
{
    public class PatientCreator
    {
        private readonly IPatientContract _patientRepository;

        public PatientCreator(IPatientContract patientRepository) => _patientRepository = patientRepository;

        public async Task<int> Run(Patient patient) => await _patientRepository.AddAsync(patient);
    }
}
