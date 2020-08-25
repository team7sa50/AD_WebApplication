using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Team7_StationeryStore.Models
{
    public class CreateAdjustmentvouncher
    {
        public string ItemCode { get; set; }
        public string EmEmployeeId { get; set; }
        public string reason { get; set; }
        public int qty { get; set; }
    }
}
