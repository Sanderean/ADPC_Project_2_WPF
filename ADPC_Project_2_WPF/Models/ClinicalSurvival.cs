﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADPC_Project_2_WPF.Models
{
    public class ClinicalSurvival
    {
        public string PatientId { get; set; }
        public int? DiseaseSpecificSurvival { get; set; }
        public int? OverallSurvival { get; set; }
        public string ClinicalStage { get; set; }
    }
}
