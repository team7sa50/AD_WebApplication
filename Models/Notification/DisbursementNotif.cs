using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Team7_StationeryStore.Models
{
    public class DisbursementNotif : Notification
    {
        public virtual Disbursement Disbursement { get; set; }

        public DisbursementNotif() :base() { 
        }
    }
}
