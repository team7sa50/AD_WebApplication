using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Team7_StationeryStore.Models
{
    public class DisbursementDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }

        public string RequisitionDetailId { get; set; }
        public string DisbursementId { get; set; }

        public int disbursedQty { get; set; }

        public virtual RequisitionDetail RequisitionDetail { get; set; }
        public virtual Disbursement Disbursement { get; set; }
        

    }
}
