using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Team7_StationeryStore.Database;
using Team7_StationeryStore.Models;
using Team7_StationeryStore.Models.Requisitions;

namespace Team7_StationeryStore.Service
{
    public class RequisitionService
    {
        protected StationeryContext dbcontext;

        public RequisitionService(StationeryContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }
        public List<Requisition> findAllRequisitionsFromStationery()
        {
            return dbcontext.requisitions.Where(x => !x.status.Equals(ReqStatus.AWAITING_APPROVAL) && !x.status.Equals(ReqStatus.REJECTED)).ToList();

        }
        public List<Requisition> findOustandingRequisitions()
        {
            return dbcontext.requisitions.Where(x => x.status.Equals(ReqStatus.OUTSTAND)).ToList();

        }

        public List<Requisition> retrieveRequisitionList(Employee employee) { 
            
            return dbcontext.requisitions.Where(x => x.DepartmentId == employee.DepartmentsId).ToList();
        }

        public Requisition findRequisition(string requisitionId) { 
            return dbcontext.requisitions.Where(x => x.Id == requisitionId).FirstOrDefault();
        }

        public void updateStatus(string requisitionId, ReqStatus status) {

            Requisition requisition = findRequisition(requisitionId);
            requisition.status = status;

        }
        public List<RequisitionDetailView> findRequisitionDetail(string requisitionId)
        {
            var items = (from c in dbcontext.requisitionDetails
                         where c.RequisitionId == requisitionId 
                         select new
                         {
                             ItemCode = c.Inventory.itemCode,
                             qty= c.RequestedQty
                         }
                      );

            List<RequisitionDetailView> data = new List<RequisitionDetailView>();
            foreach (var c in items)
            {
                RequisitionDetailView r = new RequisitionDetailView();
                r.itemCode = c.ItemCode;
                r.RequestedQty = c.qty;
                data.Add(r);

            }
            return data;
        }

    }
}
