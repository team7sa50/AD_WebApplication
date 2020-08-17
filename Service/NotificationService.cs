using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Team7_StationeryStore.Database;
using Team7_StationeryStore.Models;

namespace Team7_StationeryStore.Service
{
    public class NotificationService
    {
        protected StationeryContext dbcontext;
/*        protected DepartmentService deptService;
        protected RequisitionService requisitionService;
        protected DisbursementService disbursementService;
        protected InventoryService inventoryService;*/

        public NotificationService(StationeryContext dbcontext/*, 
                                    DepartmentService deptService,
                                    RequisitionService requisitionService, 
                                    DisbursementService disbursementService,
                                    InventoryService inventoryService*/)
        {
            this.dbcontext = dbcontext;
/*            this.deptService = deptService;
            this.requisitionService = requisitionService;
            this.disbursementService = disbursementService;
            this.inventoryService = inventoryService;*/
        }

        public void sendNotification(NotificationType notificationType,Requisition? requisition, Disbursement? disbursement,AdjustmentVoucher? adjustment) {

            Employee sender = new Employee();
            Employee reciever = new Employee();
            string subjectEmail = "";
            switch (notificationType) {
                case NotificationType.REQUISITION:
                    RequisitionNotif requisitionNotif = new RequisitionNotif();
                    subjectEmail = "New Requisition: " + requisition.Id;
                    requisitionNotif.Requisition = requisition;
                    requisitionNotif.ReceiverId = requisition.ApprovedEmployeeId;
                    requisitionNotif.SenderId = requisition.EmployeeId;
                    sender = requisition.Employee;
                    reciever = requisition.ApprovedEmployee;
                    dbcontext.Add(requisitionNotif);
                    break;
                case NotificationType.DISBURSEMENT:
                    DisbursementNotif disbursementNotif = new DisbursementNotif();
                    subjectEmail = "New Disbursement: " + disbursement.Id;

                    dbcontext.Add(disbursementNotif);
                    break;
                case NotificationType.ADJUSTMENTVOUCHER:
                    AdjustmentVoucherNotif adjustmentVoucherNotif = new AdjustmentVoucherNotif();
                    subjectEmail = "New AdjustmentVoucher: " + adjustment.Id;
                    adjustmentVoucherNotif.AdjustmentVoucher = adjustment;
                    adjustmentVoucherNotif.ReceiverId = adjustment.appEmEmployeeId;
                    adjustmentVoucherNotif.SenderId = adjustment.EmEmployeeId;
                    sender = adjustment.EmEmployee;
                    reciever = adjustment.appEmEmployee;
                    dbcontext.Add(adjustmentVoucherNotif);
                    break;
            }
            
            dbcontext.SaveChanges();
            SendEmail(sender, reciever, subjectEmail);
        }


        public void SendEmail(Employee sender, Employee reciever, string subjectEmail)
        {
            /*            var fromAddress = new MailAddress(sender.Email, "From "+sender.Name);
                        var toAddress = new MailAddress(reciever.Email, "To "+reciever.Name);
                        var fromPassword  =  sender.Password;*/
            //Demo purpose
            var fromAddress = new MailAddress("stationerystoreteam7@gmail.com", sender.Name);
            var toAddress = new MailAddress("storeclerkteam7@gmail.com", reciever.Name);
            const string fromPassword = "stationerystore";
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subjectEmail,
                Body = "Dear " + reciever.Name + ",\n"
                        + "Please take note of the request.\n"
                        + "Thank you \n"
                        + "Your regard \n"
                        + sender.Name        
            })
            {
                smtp.Send(message);
            }

        }
    }
}
