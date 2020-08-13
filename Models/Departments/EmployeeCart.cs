using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Team7_StationeryStore.Models
{
    public class EmployeeCart
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        public string EmployeeId { get; set; }
        public string InventoryId { get; set; }
        public int Qty { get; set; }
        public virtual Inventory Inventory { get; set;}
        public virtual Employee Employee { get; set; }

    }
}
