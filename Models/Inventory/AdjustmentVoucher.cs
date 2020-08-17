using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Team7_StationeryStore.Models
{
    public enum Status
    {
        APPROVED, PENDING , REJECTED
    }

    public class AdjustmentVoucher
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        [Required]
        public string InventoryId { get; set; }
        [Required]
        public string EmEmployeeId { get; set; }
        [Required]
        public string appEmEmployeeId { get; set; }
        public int qty { get;  set; }
        public DateTime date { get;  set; }
        public string reason { get;  set; }
        public Status status { get;  set; }
        public string remarks { get; set; }

        public virtual Employee EmEmployee { get;  set; }
        public virtual Employee appEmEmployee { get; set; }
        public virtual Inventory Inventory { get; set; }

        public AdjustmentVoucher() { 
            this.Id = Guid.NewGuid().ToString();
            this.date = DateTime.Now;
            this.status = Status.PENDING;
        }

    }
}
