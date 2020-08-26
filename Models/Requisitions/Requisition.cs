using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Team7_StationeryStore.Models
{
    public enum ReqStatus
    {
        AWAITING_APPROVAL,REJECTED,APPROVED,PROCESSING,COLLECTION,OUTSTAND,COMPLETED
    }
    public class Requisition
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        public string DepartmentId { get; set; }
        [ForeignKey("Employee")]
        public string EmployeeId { get; set; }
        [ForeignKey("ApprovedEmployee")]
        public string ApprovedEmployeeId { get; set; }
        public DateTime DateSubmitted { get; set; }
        public ReqStatus status { get; set; }
        public string Remarks { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Departments Department { get; set; }
        public virtual Employee ApprovedEmployee { get; set; }

        public virtual List<RequisitionDetail> RequisitionDetails { get; set; }

        public Requisition() {
            this.DateSubmitted = DateTime.Now;
            this.status = ReqStatus.AWAITING_APPROVAL;
        }

        public Requisition(string DeptCode)
        {
            this.DateSubmitted = DateTime.Now;
            this.Id = DeptCode + "_" + DateTime.Now.ToString("MM/dd/yyyy/HH:mm:ss");
            this.status = ReqStatus.AWAITING_APPROVAL;
        }
    }
}
