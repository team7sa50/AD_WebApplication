using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Team7_StationeryStore.Models
{
    public enum ReqStatus
    {
        AWAITING_APPROVAL, REJECTED,APPROVED,PROCESSING,COLLECTION,OUTSTAND,COMPLETED
    }
    public class Requisition
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        
        [ForeignKey("Employee")]
        public string EmployeeId { get; set; }

        [ForeignKey("ApprovedEmployee")]
        public string ApprovedEmployeeId { get; set; }
        public DateTime DateSubmitted { get; set; }
        public ReqStatus status { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Employee ApprovedEmployee { get; set; }
    }
}
