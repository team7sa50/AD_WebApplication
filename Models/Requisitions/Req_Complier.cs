using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Team7_StationeryStore.Models
{
    public class Req_Complier
    {
        public int Year { get; set; }
        public int Month { get; set; }
/*        public ItemCategory ItemCat { get; set; }*/

        public string InventoryCategory { get; set; }
        public int Qty { get; set; }
    }
}
