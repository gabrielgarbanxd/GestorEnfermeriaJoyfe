﻿using GestorEnfermeriaJoyfe.Domain.Patient;
using GestorEnfermeriaJoyfe.Domain.Patient.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Application.PatientApp
{
    public class PatientCreator
    {
        private readonly IPatientContract _patientRepository;

        public PatientCreator(IPatientContract patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<int> Run(Patient patient)
        {
            var newPatientId = await _patientRepository.AddAsync(patient);

            patient.SetId(new PatientId(newPatientId));

            return patient.Id.Value;
        }
    }
}
