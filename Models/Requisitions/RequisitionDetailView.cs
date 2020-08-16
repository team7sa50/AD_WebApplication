using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Team7_StationeryStore.Models.Requisitions
{
    public class RequisitionDetailView
    {
        public string RequisitionId { get; set; }
        public string itemCode { get; set; }
        public int RequestedQty { get; set; }
    }
}
