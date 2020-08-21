using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Team7_StationeryStore.Analytics.ML.Objects
{
    public class CarInventoryPrediction
    {
        //Class containing the properties mapped to the prediction output 
        public bool Label { get; set; }

        //Classification output 
        public bool PredictedLabel { get; set; }

        public float Score { get; set; }

        //The probability 
        public float Probability { get; set; }
    }
}
