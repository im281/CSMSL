﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSMSL.Chemistry;

namespace CSMSL.Analysis.Quantitation
{
    public interface IQuantitationChannel: IChemicalFormula, IMass
    {   
        bool IsSequenceDependent { get; }
        Mass ReporterMass { get; }
    }
}