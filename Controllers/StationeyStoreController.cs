using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Team7_StationeryStore.Database;
using Team7_StationeryStore.Models;

namespace Team7_StationeryStore.Controllers
{
    public class StationeryStoreController : Controller
    {
        protected StationeryContext dbcontext;

        public StationeryStoreController(StationeryContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public IActionResult Index()
        {
            System.Diagnostics.Debug.WriteLine("Reached Stationery Store Controller");
            return RedirectToAction("viewRetrieval", "StationeryStore");
        }

        public IActionResult viewRetrieval()
        {
            List<Requisition> selectedReq = (from x in dbcontext.requisitions
                                            select x).ToList();

            //Contains all the Requisition Details 
            List<RequisitionDetail> selectedReqDetail = new List<RequisitionDetail>(); 
            
            foreach (Requisition r in selectedReq)
            {
                List<RequisitionDetail> rds = (from x in dbcontext.requisitionDetails
                                               where x.Requisition == r
                                               select x).ToList();
                foreach (RequisitionDetail rd in rds)
                {
                    selectedReqDetail.Add(rd);
                }
            }

            //Group by unqiue items and total count
            Dictionary<string, int> totalItemQty = new Dictionary<string, int>();
            Dictionary<string, List<RequisitionDetail>> reqPerIt = new Dictionary<string, List<RequisitionDetail>>();
          
            foreach (RequisitionDetail rd in selectedReqDetail)
            {
                string item = rd.Inventory.description;
                if (!totalItemQty.ContainsKey(item))
                {
                    totalItemQty.Add(item, rd.RequestedQty);
                    reqPerIt.Add(item, new List<RequisitionDetail>());
                    reqPerIt[item].Add(rd);
                }
                else if(totalItemQty.ContainsKey(item))
                {
                    int qty = totalItemQty[item];
                    qty += rd.RequestedQty;
                    totalItemQty[item] = qty;
                    reqPerIt[item].Add(rd);
                }
            }
            ViewData["totalItemQty"] = totalItemQty;
            recommendQty(reqPerIt);

            System.Diagnostics.Debug.WriteLine("Start getting requisitions for one item");
            Dictionary<string, List<RequisitionDetail>> reqPerDept = new Dictionary<string, List<RequisitionDetail>>();
            string deptName; 
            //loop through each item - requisition detail pair 
            foreach(var rq in reqPerIt)
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
            ViewData["reqPerDept"] = reqPerDept;
            return View();        
        }

        public void recommendQty(Dictionary<string, List<RequisitionDetail>> reqPerIt)
        {

            int itemQty;
            int itemNeeded; 
            foreach(var r in reqPerIt)
            {
                Inventory item = (from i in dbcontext.inventories
                                  where i.description == r.Key
                                  select i).FirstOrDefault();
                itemQty = item.stock;
                foreach(var rd in r.Value)
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
    }
}
