using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ML.Data;

namespace Team7_StationeryStore.Analytics.ML.Objects
{
    public class CarInventory
    {
        // Class containing containers to hold formatted raw data 
        //Don't forget to increment the array index as follows 
        [LoadColumn(0)]
        public float RequestedQty { get; set; }

        [LoadColumn(1)]
        public float InventoryQty { get; set; }

        [LoadColumn(2)]
        public float Month { get; set; }

        [LoadColumn(3)]
        public float Year { get; set; }

        [LoadColumn(4)]
        public bool Label { get; set; }
    }
}
