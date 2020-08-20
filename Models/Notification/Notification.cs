using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Team7_StationeryStore.Models
{
    public enum NotificationType { 
        ADJUSTMENTVOUCHER,REQUISITION,DISBURSEMENT
    }
    public class Notification
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        public bool read { get; set; }
        [ForeignKey("Sender")]
        public string SenderId { get; set; }
        [ForeignKey("Receiver")]
        public string ReceiverId { get; set; }
        public DateTime date { get; set; }
        public string typeId { get; set; }
        public NotificationType type { get; set; }
        public virtual Employee Sender { get; set; }
        public virtual Employee Receiver { get; set; }

        public Notification() {
            this.Id = Guid.NewGuid().ToString();
            this.date = DateTime.Now;
            this.read = false;
        }
    }
}
    