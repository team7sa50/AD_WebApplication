using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Team7_StationeryStore.Database;
using Team7_StationeryStore.Models;
using Team7_StationeryStore.Service;

namespace Team7_StationeryStore.ApiControllers
{
    [ApiController]
    public class StationeryStoreApiController : ControllerBase
    {

        protected StationeryContext dbcontext;
        protected RequisitionService reqService;
        protected InventoryService invService;
        protected DepartmentService deptService;
        protected DisbursementService disService;
        protected NotificationService notiService;
        protected RetrievalService rservice;

        public StationeryStoreApiController(RequisitionService reqService, InventoryService invService, DepartmentService deptService, DisbursementService disService, NotificationService notiService, StationeryContext dbcontext, RetrievalService rservice)
        {
            this.deptService = deptService;
            this.invService = invService;
            this.reqService = reqService;
            this.dbcontext = dbcontext;
            this.disService = disService;
            this.rservice = rservice;
            this.notiService = notiService;
        }
        [HttpGet]
        [Route("api/[controller]/viewInventories")]
        public ActionResult viewInventories()
        {
            List<Inventory> inventories = invService.getAllInventories();
            return Content(JsonConvert.SerializeObject(inventories));
        }

        [HttpGet]
        [Route("api/[controller]/viewInventoryDetail")]
        public ActionResult viewInventoryDetail(string invId)
        {
            Inventory inventory = invService.retrieveInventory(invId);
            return Content(JsonConvert.SerializeObject(inventory));
        }
        [HttpGet]
        [Route("api/[controller]/viewSuppliers")]
        public ActionResult viewSuppliers()
        {
            List<Supplier> suppliers = invService.getAllSuppliers();
            return Content(JsonConvert.SerializeObject(suppliers));
        }
        [HttpGet]
        [Route("api/[controller]/viewSupplierDetail")]
        public IActionResult viewSupplier(string supplierId)
        {
            Supplier supplier = invService.getSupplier(supplierId);
            return Content(JsonConvert.SerializeObject(supplier));
        }
        [HttpGet]
        [Route("api/[controller]/viewDepartments")]
        public ActionResult viewDepartments()
        {
            var items = (from d in dbcontext.departments
                                     select new
                         {
                             Id = d.Id,
                             DeptCode=d.DeptCode,
                             DepartmentName=d.DeptName,
                             ContactName=d.ContactName,
                             Telephone=d.PhoneNumber,
                             FaxNo=d.FaxNumber,
                             HeadName=d.DeptHead,
                             RepName=d.Representative,
                             CollectionPoint=d.CollectionPoint.Location
                         }
          );
            return Content(JsonConvert.SerializeObject(items, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
        }
        [HttpGet]
        [Route("api/[controller]/viewDepartmentDetail")]
        public IActionResult viewDepartmentDetail(string deptId)
        {
            Departments department = invService.getDepartmentDetail(deptId);
            return Content(JsonConvert.SerializeObject(department, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
        }
        [HttpGet]
        [Route("api/[controller]/viewAllItemCodes")]
        public IActionResult viewAllItemCodes()
        {
            var items = (from i in dbcontext.inventories
                         select new
                         {
                           ItemCode=i.itemCode
                         }
                        );
            return Content(JsonConvert.SerializeObject(items));
        }
        [HttpPost]
        [Route("api/[controller]/viewPurchaseOrderByEmpId")]
        public IActionResult viewPurchaseOrderByEmpId([FromBody]PurchaseOrder value)
        {
            var po = (from p in dbcontext.purchaseOrders
                         where p.EmployeeId==value.EmployeeId
                         select new
                         {
                             Id=p.Id,
                             Supplier = p.Supplier.name,
                             Date=p.date,
                             Status=p.status.ToString()
                         }
            );
            return Content(JsonConvert.SerializeObject(po, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
        }
        [HttpPost]
        [Route("api/[controller]/createAdjustmentVouncher")]
        public ActionResult crateAdjustmentVouncher([FromBody]CreateAdjustmentvouncher value)
        {
            Inventory inventory = invService.findInventory(value.ItemCode);
            invService.CreateAdjustmentVoucher(value.EmEmployeeId,inventory.Id, value.qty, value.reason);
            Object response = new
            {
                message = "Successfully created",
                code = HttpStatusCode.OK

            };
            return Content(JsonConvert.SerializeObject(response));
        }

        //Hello Zijian, this is the POST disbursement API 
        // It will change the status to "processing" automatically, don't need to post status again 
        [HttpPost]
        [Route("api/[controller]/generateDisbursement")]
        public ActionResult generateDisbursement([FromBody]List<Requisition> requisitions)
        {
            //Transfer retrieved requests here
            string userId = "Mary";
            List<Requisition> selectedReq = reqService.getRequisitionsByRequisitionIds(requisitions);
            reqService.updateRequisitionStatus(selectedReq);
            List<RequisitionDetail> selectedReqDetail = rservice.getRequisitionDetail(selectedReq);
            //Convert request to disbursement 
            Dictionary<Departments, List<RequisitionDetail>> requisitionsForDepartment = disService.sortRequisitionByDept(selectedReqDetail);
            disService.saveRequisitionsAsDisbursement(userId,requisitionsForDepartment);
            Object response = new
            {
                message = "Successfully created",
                code = HttpStatusCode.OK

            };
            return Content(JsonConvert.SerializeObject(response));
        }

        ////Hello Zijian, this is the GET disbursement API; the web display all disbursements, not just the generated one so we follow 
        [HttpGet]
        [Route("api/[controller]/viewAllDisbursements")]
        public IActionResult viewAllDisbursements()
        {
            List<Disbursement> disbursements = disService.retrieveDisbursements();
            return Content(JsonConvert.SerializeObject(disbursements, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
        }


        [HttpGet]
        [Route("api/[controller]/viewApprovedRequisitions")]
        public IActionResult viewApprovedRequisitions()
        {
            var requisitions = (from r in dbcontext.requisitions
                      where r.status == ReqStatus.APPROVED
                      select new
                      {
                          Id = r.Id,
                          Date = r.DateSubmitted,
                          Status = r.status.ToString()
                      }
          );
            return Content(JsonConvert.SerializeObject(requisitions, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
        }
        [HttpPost]
        [Route("api/[controller]/getAdjustmentVoucherByEmpId")]
        public IActionResult getAdjustmentVoucherByEmpId([FromBody]AdjustmentVoucher value)
        {
            var adjustmentVouchers = (from a in dbcontext.adjustmentVouchers
                                where a.EmEmployeeId == value.EmEmployeeId
                                select new
                                {
                                    Description = a.Inventory.description,
                                    Date = a.date,
                                    Quantity=a.qty,
                                    Reason=a.reason,
                                    Status = a.status.ToString(),
                                } );
            return Content(JsonConvert.SerializeObject(adjustmentVouchers, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));

        }
    }
}