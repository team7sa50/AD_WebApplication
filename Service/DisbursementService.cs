using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Team7_StationeryStore.Database;
using Team7_StationeryStore.Models;

namespace Team7_StationeryStore.Service
{
    public class DisbursementService
    {
        protected StationeryContext dbcontext;
        protected DepartmentService deptService;
        protected RequisitionService reqSerivce;
        protected InventoryService invService;

        public DisbursementService(StationeryContext dbcontext, DepartmentService deptService, RequisitionService reqService, InventoryService invService)
        {
            this.dbcontext = dbcontext;
            this.deptService = deptService;
            this.reqSerivce = reqService;
            this.invService = invService;
        }

        public List<Disbursement> findLatestDisbursements()
        {
/*            List<Disbursement> rq = (from r in dbcontext.disbursements
                                    select r).ToList();
            List<Disbursement> result = new List<Disbursement>();
            for (int i = 0; i < 5; i++)
            {
                result.Add(rq[i]);
            }

            return result;*/

            return dbcontext.disbursements.OrderByDescending(x => x.GeneratedDate).Take(5).ToList();
        }
        public List<Disbursement> retrieveDisbursementByDept(string deptId)
        {
            return dbcontext.disbursements.Where(x => x.DepartmentsId == deptId && (x.status==DisbusementStatus.DELIVERED || x.status==DisbusementStatus.COMPLETED)).ToList();
        }
        public List<Disbursement> retrieveDisbursements()
        {
            return dbcontext.disbursements.Where(x => x.GeneratedDate.Date == DateTime.Today.Date).ToList();
        }

        public List<DisbursementDetail> retrieveDisbursementDetails(string disId)
        {

            return dbcontext.disbursementDetails.Where(x => x.DisbursementId == disId).ToList();
        }

        public List<DisbursementDetailView> retrieveDisbursmentDetailsView(string disId)
        {
            List<DisbursementDetail> retrievedDisbursement = retrieveDisbursementDetails(disId);
            List<DisbursementDetailView> disbursementDetailViews = new List<DisbursementDetailView>();
            foreach (var i in retrievedDisbursement)
            {

                DisbursementDetailView disDview = new DisbursementDetailView(i.DisbursementId,
                                                                        i.RequisitionDetail.Inventory.itemCode,
                                                                        i.disbursedQty,
                                                                        i.Disbursement.status,
                                                                        i.Disbursement.CollectionDate);
                disbursementDetailViews.Add(disDview);
            }
            return disbursementDetailViews;
        }

        public Dictionary<Departments, List<RequisitionDetail>> sortRequisitionByDept(List<RequisitionDetail> selectedReqDetail)
        {
            Dictionary<Departments, List<RequisitionDetail>> requisitionsForDepartment = new Dictionary<Departments, List<RequisitionDetail>>();
            foreach (var r in selectedReqDetail)
            {
                if (!requisitionsForDepartment.ContainsKey(r.Requisition.Department))
                {
                    requisitionsForDepartment.Add(r.Requisition.Department, new List<RequisitionDetail>());
                    requisitionsForDepartment[r.Requisition.Department].Add(r);
                }
                else if (requisitionsForDepartment.ContainsKey(r.Requisition.Department))
                {
                    requisitionsForDepartment[r.Requisition.Department].Add(r);
                }
            }
            return requisitionsForDepartment;
        }

        public void saveRequisitionsAsDisbursement(string userId,Dictionary<Departments, List<RequisitionDetail>> requisitionsForDepartment)
        {
            Employee employee = deptService.findEmployeeById(userId);

            foreach (var dept in requisitionsForDepartment)
            {
                //Create new disbursement
                Disbursement d = new Disbursement();
                d.Id = Guid.NewGuid().ToString();
                d.GeneratedDate = DateTime.Now;
                d.CollectionDate = DateTime.Now.AddDays(1);
                d.Departments = dept.Key;
                d.storeClerk = employee;
                d.DepartmentsId = dept.Key.Id;
                d.status = DisbusementStatus.PENDING;
                dbcontext.Add(d);
                dbcontext.SaveChanges();

                //In each new disbursement, create new disbursement
                foreach (var req in dept.Value)
                {
                    DisbursementDetail ddetail = new DisbursementDetail();
                    ddetail.Id = Guid.NewGuid().ToString();
                    ddetail.DisbursementId = d.Id;
                    ddetail.Disbursement = d;
                    ddetail.RequisitionDetail = req;
                    ddetail.disbursedQty = req.DistributedQty;
                    dbcontext.Add(ddetail);
                    dbcontext.SaveChanges();
                }
            }
        }

       /* public Dictionary<Inventory, int> compileRequisitionDetails(List<RequisitionDetail> requisitionDetails)
        {
            Dictionary<Inventory, int> compliedList = new Dictionary<Inventory, int>();
            foreach (var req in requisitionDetails)
            {
                if (compliedList.ContainsKey(req.Inventory))
                {
                    compliedList[req.Inventory] += req.RequestedQty;
                }
                else
                {
                    compliedList.Add(req.Inventory, req.RequestedQty);
                }
            }
            return compliedList;
        }*/

        public List<Disbursement> getAllPendingDisbursements()
        {
            List<Disbursement> dibs = (from d in dbcontext.disbursements
                                       where d.status == DisbusementStatus.PENDING
                                       select d).ToList();
            return dibs;
        }

        public List<Disbursement> getAllCompletedDisbursements()
        {
            List<Disbursement> dibCompleted = (from d in dbcontext.disbursements
                                               where d.status == DisbusementStatus.COMPLETED  || d.status==DisbusementStatus.DELIVERED
                                               select d).ToList();
            return dibCompleted;
        }


        public Disbursement findDisbursementById(string id)
        {
            Disbursement d = (from ds in dbcontext.disbursements
                              where ds.Id == id
                              select ds).FirstOrDefault();
            return d;
        }



        // Unsure how is the Andriod going to pass the disbursed qty
        public void acknowledgeDisbursement(string disId, List<Disbursement_Detail> disDetailList) {

            Disbursement disbursement = findDisbursementById(disId);
            //Group the disbursement Details to according to itemId and sort by earliest to latest submition date
            Dictionary<string, List<DisbursementDetail>> list_disDetails = GroupSortDisDetailsByItemAndEarliestDate(disbursement);

            //Distribute disbursed qty as per items into respectively disbursement_detail & requisition_detail
            foreach(var d in disDetailList){
               distributeItemQty(list_disDetails[d.itemId],d.disbursedQty);
            }

            List<Requisition> requisitions = disbursement.Requisitions.ToList();

            //Update requisition while checking its fufilement
            foreach (var r in requisitions.Where(r => !reqSerivce.isPartialFufiled(r)).Select(r => r))
            {
                r.status = ReqStatus.COMPLETED;
            }
            dbcontext.Update(requisitions);
            dbcontext.SaveChanges();
        }

        public Dictionary<string, List<DisbursementDetail>> GroupSortDisDetailsByItemAndEarliestDate(Disbursement disbursement) {  
            return disbursement.DisbursementDetails.OrderBy(x => x.RequisitionDetail.Requisition.DateSubmitted)
                                                   .GroupBy(x => x.RequisitionDetail.InventoryId)
                                                   .ToDictionary(x => x.Key, x => x.ToList());
        }

        //Distribution of item qty among requisitions reference to the disbursement
        public void distributeItemQty(List<DisbursementDetail> list,int qty){
            
            foreach (var i in list) {

                if (qty > i.RequisitionDetail.RequestedQty)
                {
                    qty -= i.RequisitionDetail.RequestedQty;
                    // Update Requisition Detail
                    i.RequisitionDetail.DistributedQty = i.RequisitionDetail.RequestedQty;
                    // Update Disbursement Detail
                    i.disbursedQty = i.RequisitionDetail.RequestedQty;
                }
                // Insufficient amount disbursed qty.
                else {
                    // Update Requisition Detail
                    i.RequisitionDetail.DistributedQty = i.RequisitionDetail.RequestedQty - qty;
                    // Update Disbursement Detail
                    i.disbursedQty = qty;
                    break;
                }
            }
            dbcontext.Update(list);
        }

        
        
    }
}
