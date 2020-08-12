using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Team7_StationeryStore.Models
{
    public class RetrievalDetails
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        [Required]
        public string RetrievalId { get;  set; }
        [Required]
        public string RequisitionId { get; set; }
        public virtual Retrieval Retrieval { get; set; }
        public virtual Requisition Requisition { get; set; }
    }
}
