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

        public DisbursementService(StationeryContext dbcontext, DepartmentService deptService)
        {
            this.dbcontext = dbcontext;
            this.deptService = deptService;
        }

        public List<Disbursement> retrieveDisbursementByDept(string deptId) {
            return dbcontext.disbursements.Where(x => x.DepartmentsId == deptId).ToList();
        }

        public List<DisbursementDetail> retrieveDisbursementDetails(string disId) {

            return dbcontext.disbursementDetails.Where(x => x.DisbursementId == disId).ToList();
        }

        public List<DisbursementDetailView> retrieveDisbursmentDetailsView(string disId) {
            List<DisbursementDetail> retrievedDisbursement = retrieveDisbursementDetails(disId);
            List<DisbursementDetailView> disbursementDetailViews = new List<DisbursementDetailView>();
            foreach (var i in retrievedDisbursement) {

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

        public void saveRequisitionsAsDisbursement(Dictionary<Departments, List<RequisitionDetail>> requisitionsForDepartment)
        {
            foreach (var dept in requisitionsForDepartment)
            {
                Disbursement d = new Disbursement();
                d.Id = Guid.NewGuid().ToString();
                d.GeneratedDate = DateTime.Now;
                d.CollectionDate = DateTime.Now.AddDays(1);
                d.Departments = dept.Key;
                d.DepartmentsId = dept.Key.Id;
                d.status = DisbusementStatus.PENDING;
                dbcontext.Add(d);
                dbcontext.SaveChanges();

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

        public List<Disbursement> getAllPendingDisbursements()
        {
            List<Disbursement> dibs = (from d in dbcontext.disbursements
                                       where d.status == DisbusementStatus.PENDING
                                       select d).ToList();
            return dibs;
        }

        public List<Disbursement> getAllCompletedDisbursements() {
             List<Disbursement> dibCompleted = (from d in dbcontext.disbursements
                                               where d.status == DisbusementStatus.COMPLETED
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

    }
}
