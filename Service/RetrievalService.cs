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
    public class RetrievalService
    {
        protected StationeryContext dbcontext;
        public RetrievalService(StationeryContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public List<RequisitionDetail> getRequisitionDetail(List<Requisition> selectedReq)
        {
            System.Diagnostics.Debug.WriteLine("Reached Get Req Service");
            List<RequisitionDetail> selectedReqDetail = new List<RequisitionDetail>();
            foreach (Requisition r in selectedReq)
            {
                System.Diagnostics.Debug.WriteLine("Outer Loop Count: " + r.Id);
                List<RequisitionDetail> rds = (from x in dbcontext.requisitionDetails
                                               where x.Requisition.Id == r.Id
                                               select x).ToList();
                
                foreach (RequisitionDetail rd in rds)
                {
                    System.Diagnostics.Debug.WriteLine("Service Count: " + 1);
                    selectedReqDetail.Add(rd);
                }
            }

            return selectedReqDetail;
        }

        public Dictionary<string, int> getTotalQtyPerItem(List<RequisitionDetail> selectedReqDetail)
        {
            Dictionary<string, int> totalItemQty = new Dictionary<string, int>();
            
            foreach (RequisitionDetail rd in selectedReqDetail)
            {
                string item = rd.Inventory.description;
                if (!totalItemQty.ContainsKey(item))
                {
                    totalItemQty.Add(item, rd.RequestedQty);
                }
                else if (totalItemQty.ContainsKey(item))
                {
                    int qty = totalItemQty[item];
                    qty += rd.RequestedQty;
                    totalItemQty[item] = qty;
                }
            }

            return totalItemQty;

        }

        public Dictionary<string, List<RequisitionDetail>> getReqDetailPerItem(List<RequisitionDetail> selectedReqDetail)
        {
            Dictionary<string, List<RequisitionDetail>> reqPerIt = new Dictionary<string, List<RequisitionDetail>>();

            foreach (RequisitionDetail rd in selectedReqDetail)
            {
                string item = rd.Inventory.description;
                if (!reqPerIt.ContainsKey(item))
                {
                    reqPerIt.Add(item, new List<RequisitionDetail>());
                    reqPerIt[item].Add(rd);
                }
                else if (reqPerIt.ContainsKey(item))
                {
                    reqPerIt[item].Add(rd);
                }
            }

            return reqPerIt;

        }

        public void recommendQty(Dictionary<string, List<RequisitionDetail>> reqPerIt)
        {

            int itemQty;
            int itemNeeded;
            foreach (var r in reqPerIt)
            {
                Inventory item = (from i in dbcontext.inventories
                                  where i.description == r.Key
                                  select i).FirstOrDefault();
                itemQty = item.stock;
                foreach (var rd in r.Value)
                {
                    itemNeeded = rd.RequestedQty;
                    if (itemQty >= itemNeeded)
                    {
                        rd.DistributedQty = itemNeeded;
                    }
                    else if (itemQty < itemNeeded)
                    {
                        rd.DistributedQty = itemQty;
                    }
                    dbcontext.SaveChanges();
                }
            };
        }

        public Dictionary<string, List<RequisitionDetail>> getReqPerDeptPerItem(Dictionary<string, List<RequisitionDetail>> reqPerIt)
        {
            Dictionary<string, List<RequisitionDetail>> reqPerDept = new Dictionary<string, List<RequisitionDetail>>();
            string deptName;
            //loop through each item - requisition detail pair 
            foreach (var rq in reqPerIt)
            {
                foreach (var r in rq.Value)
                {
                    deptName = r.Requisition.Employee.Departments.DeptName;
                    if (!reqPerDept.ContainsKey(deptName))
                    {
                        //find 
                        reqPerDept.Add(deptName, new List<RequisitionDetail>());
                        reqPerDept[deptName].Add(r);
                    }
                    else if (reqPerDept.ContainsKey(deptName))
                    {
                        reqPerDept[deptName].Add(r);
                    }
                }
            }
            return reqPerDept;
        }
    }
}
