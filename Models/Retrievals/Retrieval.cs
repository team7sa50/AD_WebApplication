using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Team7_StationeryStore.Models
{
    public class Retrieval
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get;  set; }
        [Required]
        public DateTime RetrievalDate { get; set; }
        [Required]
        public string EmployeeId { get; set; }
        public virtual Employee Employee { get;  set; }
        public virtual ICollection<RetrievalDetails> RetrievalDetails { get;  set; }
    }
}
