using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Team7_StationeryStore.Models
{
    public class RequisitionNotif : Notification
    {
        public virtual Requisition Requisition { get; set; }

        public RequisitionNotif():base(){
        }

    } 
}

