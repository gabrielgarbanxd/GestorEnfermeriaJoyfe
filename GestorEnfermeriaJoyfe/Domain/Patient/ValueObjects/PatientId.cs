﻿using GestorEnfermeriaJoyfe.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorEnfermeriaJoyfe.Domain.Patient.ValueObjects
{
    public class PatientId : IdValueObject
    {
        public PatientId(int value) : base(value){}
    }
}
