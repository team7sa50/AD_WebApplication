using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Team7_StationeryStore.Models
{
    public class PurchaseOrderQuantity
    {
        public int Month { get; set; }
        public int quantity { get; set; }

    }
}
