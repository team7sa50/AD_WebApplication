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
    }
}
