using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Team7_StationeryStore.Models
{
    public class AdjustmentVoucherNotif : Notification
    {
        public virtual AdjustmentVoucher AdjustmentVoucher { get; set; }

        public AdjustmentVoucherNotif():base() { }
    }
}
