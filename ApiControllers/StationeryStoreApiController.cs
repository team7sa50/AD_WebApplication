﻿using System;
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

        public StationeryStoreApiController(RequisitionService reqService, InventoryService invService, DepartmentService deptService, DisbursementService disService, NotificationService notiService, StationeryContext dbcontext)
        {
            this.deptService = deptService;
            this.invService = invService;
            this.reqService = reqService;
            this.dbcontext = dbcontext;
            this.disService = disService;
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
            List<Departments> departments = invService.getAllDepartments();
            return Content(JsonConvert.SerializeObject(departments, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
        }
        [HttpGet]
        [Route("api/[controller]/viewDepartmentDetail")]
        public IActionResult viewDepartmentDetail(string deptId)
        {
            Departments department = invService.getDepartmentDetail(deptId);
            return Content(JsonConvert.SerializeObject(department, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
        }
        [HttpPost]
        [Route("api/[controller]/createAdjustmentVouncher")]
        public ActionResult crateAdjustmentVouncher([FromBody]AdjustmentVoucher value)
        {
            AdjustmentVoucher adjustmentVoucher = new AdjustmentVoucher();
            adjustmentVoucher.Id = Guid.NewGuid().ToString();
            adjustmentVoucher.InventoryId = value.InventoryId;
            adjustmentVoucher.EmEmployeeId = value.EmEmployeeId;
            adjustmentVoucher.appEmEmployeeId = value.appEmEmployeeId;
            adjustmentVoucher.qty = value.qty;
            adjustmentVoucher.date = value.date;
            adjustmentVoucher.reason = value.reason;
            adjustmentVoucher.status = value.status;
            adjustmentVoucher.remarks = value.remarks;
            dbcontext.Add(adjustmentVoucher);
            dbcontext.SaveChanges();
            Object response = new
            {
                message = "Successfully created",
                code = HttpStatusCode.OK

            };
            return Content(JsonConvert.SerializeObject(response));
        }
        [HttpGet]
        [Route("api/[controller]/viewAllDisbursements")]
        public IActionResult viewAllDisbursements()
        {
            List<Disbursement> disbursements = disService.retrieveDisbursements();
            return Content(JsonConvert.SerializeObject(disbursements, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
        }
    }
}