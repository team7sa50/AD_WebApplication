using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Team7_StationeryStore.Database;
using Team7_StationeryStore.Models;

namespace Team7_StationeryStore.Service
{
    public class RequisitionService
    {
        protected StationeryContext dbcontext;

        public RequisitionService(StationeryContext dbcontext)
        {
            this.dbcontext = dbcontext;
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

    }
}
