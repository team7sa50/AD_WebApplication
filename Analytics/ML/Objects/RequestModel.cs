using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Team7_StationeryStore.Analytics.ML.Objects
{
    public class RequestModel
    {
        public string RequestedQty { get; set; }

        public string InventoryQty { get; set; }

        public string Month { get; set; }

        public string Year { get; set; }
    }
}
