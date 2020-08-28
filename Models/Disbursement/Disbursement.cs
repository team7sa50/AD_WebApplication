using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Team7_StationeryStore.Models
{
    public enum DisbusementStatus { 
        PENDING,REJECT,COMPLETED,DELIVERED
    }
    public class Disbursement
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        public string DepartmentsId { get; set; }
        public DateTime GeneratedDate { get; set; }
        public DateTime CollectionDate { get; set; }
        public DisbusementStatus status { get; set; }
        public virtual Departments Departments { get; set; }
        public virtual ICollection<DisbursementDetail> DisbursementDetails { get; set; }
        public virtual ICollection<Requisition> Requisitions { get; set; }
        public virtual Employee storeClerk { get; set; }
    }
}
