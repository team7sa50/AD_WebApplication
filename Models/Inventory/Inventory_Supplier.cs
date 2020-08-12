using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Team7_StationeryStore.Models
{
    public class Inventory_Supplier
    {

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        [Required]
        public string InventoryItemId { get; set; }
        public string SupplierId { get;  set; }
        public int qtyOrdered { get; set; }
        public virtual Inventory Inventory { get;  set; }
        public virtual Supplier Supplier { get;  set; }

    }
}
