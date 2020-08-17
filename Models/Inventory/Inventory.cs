using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Team7_StationeryStore.Models
{
    public class Inventory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        [Required]
        public string ItemCategoryId { get; set; }
        public string itemCode { get; set; }
        public string description { get;  set; }
        public double price { get; set; }
        public int stock { get;  set; }
        public string measurementUnit { get;  set; }
        public string location { get; set; }
        public int reorderLevel { get;  set; }
        public int reorderQty { get;  set; }
        public virtual ItemCategory ItemCategory { get; set; }

    }
}
