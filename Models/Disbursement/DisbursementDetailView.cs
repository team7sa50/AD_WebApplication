using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Team7_StationeryStore.Models
{
    public class DisbursementDetailView
    {
        private string disbursementId;
        private string itemCode;
        private DisbusementStatus DisbusementStatus;
        private int recievedQty;
        private DateTime collectionDate;

        public DisbursementDetailView(string disbursementId,string itemCode,int recievedQty, DisbusementStatus DisbusementStatus, DateTime collectionDate) {
            this.disbursementId = disbursementId;
            this.itemCode = itemCode;
            this.recievedQty = recievedQty;
            this.DisbusementStatus = DisbusementStatus;
            this.collectionDate = collectionDate;
        }
        
    }
}
