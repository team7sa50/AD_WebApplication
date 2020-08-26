using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Team7_StationeryStore.Models
{
    public class RequisitionDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        public string RequisitionId { get; set; }
        public string InventoryId { get; set; }
        public int RequestedQty { get; set; }
        public int DistributedQty { get; set; }
        public virtual Requisition Requisition { get; set; }
        public virtual Inventory Inventory { get; set; }
    }
}
