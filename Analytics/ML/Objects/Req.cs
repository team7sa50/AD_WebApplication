using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Team7_StationeryStore.Analytics.ML.Objects
{
    public class Req
    {
        [LoadColumn(0)]
        public int Month { get; set; }

/*        [LoadColumn(1)]
        public string Department { get; set; }

        [LoadColumn(2)]
        public string Item { get; set; }*/

        [LoadColumn(1)]
        public float Qty { get; set; }

    }
}
