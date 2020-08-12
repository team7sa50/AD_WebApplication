using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Team7_StationeryStore.Models
{
    public enum POStatus
    {
         PENDING,DELIVERED
    }

    public class PurchaseOrder
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        [Required]
        public string SupplierId { get; set; }
        [Required]
        public string EmployeeId { get; set; }
        public DateTime date { get;  set; }
        public POStatus status { get; set; }
        public virtual Employee Employee { get;  set; }
        public virtual Supplier Supplier { get;  set; }
        public virtual ICollection<PurchaseOrderDetails> PurchaseOrderDetails { get; set; }

    }
}
