using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Team7_StationeryStore.Database;
using Team7_StationeryStore.Models;
using Microsoft.AspNetCore.Http;
using Team7_StationeryStore.Service;

namespace Team7_StationeryStore.Controllers
{
    public class PurchaseOrderController : Controller
    {

        protected StationeryContext dbcontext;
        protected RequisitionService reqService;
        protected InventoryService invService;
        protected DepartmentService deptService;
        protected DisbursementService disService;
        public PurchaseOrderController(StationeryContext dbcontext, RequisitionService reqService, InventoryService invService, DepartmentService deptService, DisbursementService disService)
        {
            this.deptService = deptService;
            this.invService = invService;
            this.reqService = reqService;
            this.dbcontext = dbcontext;
            this.disService = disService;
        }

        public IActionResult SelectSupplier()
        {
            string userid = HttpContext.Session.GetString("userId");
            List<Supplier> suppliers = invService.getAllSuppliers();
            Employee emp = deptService.findEmployeeById(userid);
            
           /* ViewData["username"] = emp.Name;*/
            ViewData["suppliers"] = suppliers;
            return View();

        }
        public IActionResult PurchaseOrder(string supplier)
        {
            if (supplier == null) {
                supplier = HttpContext.Session.GetString("supplier");
            }
            string userid = HttpContext.Session.GetString("userId");
            List<Inventory> stationeryCatalogue = invService.retrieveCatalogueFromClerk(supplier);
            List<ItemCategory> categories = invService.retrieveCategories();
            Employee emp = deptService.findEmployeeById(userid);
            Supplier s = invService.getSupplier(supplier);
            HttpContext.Session.SetString("supplier", s.Id);
            ViewData["stationeryCatalgoue"] = stationeryCatalogue;
            ViewData["categories"] = categories;
            /*ViewData["username"] = emp.Name;*/
            ViewData["supplier"] = s;
            return View();
        }
        public IActionResult ViewCart(string supplier)
        {
            if (supplier == null)
            {
                supplier = HttpContext.Session.GetString("supplier");
            }
            Supplier s = invService.getSupplier(supplier);
            string userid = HttpContext.Session.GetString("userId");
            List<PurchaseCart> purchaseCarts = invService.retrievePurchaseCart(userid);
            Employee emp = deptService.findEmployeeById(userid);
            ViewData["purchaseCarts"] = purchaseCarts;
            ViewData["username"] = emp.Name;
            ViewData["supplier"] = s;
            ViewData["userid"] = userid;
            return View();
        }
        public IActionResult AddToCart(string itemId, int quantity)
        {
            string userid = HttpContext.Session.GetString("userId");
            var User = dbcontext.employees.Where(x => x.Id == userid).FirstOrDefault();
            AddPurchaseItem(userid, itemId, quantity);
            return RedirectToAction("PurchaseOrder");
        }
        public void AddPurchaseItem(string userid, string itemid, int qty)
        {
            var oldcartItem = dbcontext.purchaseCarts
                .Where(x => x.EmployeeId == userid && x.InventoryId == itemid)
                .FirstOrDefault();
            if (oldcartItem == null)
            {
                var cartItem = new PurchaseCart()
                {
                    Id = Guid.NewGuid().ToString(),
                    EmployeeId = userid,
                    InventoryId = itemid,
                    Qty = qty,
                    Inventory = dbcontext.inventories.SingleOrDefault(p => p.Id == itemid)
                };
                dbcontext.purchaseCarts.Add(cartItem);
                dbcontext.SaveChanges();

            }
            else
            {
                oldcartItem.Qty = qty;
                dbcontext.Update(oldcartItem);
                dbcontext.SaveChanges();
            }
        }

        public IActionResult RaisePurchaseOrder(string supplier)
        {
            if (supplier == null)
            {
                supplier = HttpContext.Session.GetString("supplier");
            }
            invService.CreatePurchaseOrder(HttpContext.Session.GetString("userId"),supplier);
            HttpContext.Session.Remove("supplier");
            return RedirectToAction("ViewAllPurchaseOrders");
        }

        public IActionResult RemoveItem(string userid, string itemId)
        {
            var cartItem = dbcontext.purchaseCarts
                .Where(x => x.EmployeeId == userid && x.InventoryId == itemId)
                .FirstOrDefault();
            if (cartItem != null)
            {
                dbcontext.purchaseCarts.Remove(cartItem);
            }

            dbcontext.SaveChanges();
            return RedirectToAction("ViewCart");
        }
        public IActionResult RemoveAllItems()
        {
            string userid = HttpContext.Session.GetString("userId");
            var cartItem = dbcontext.purchaseCarts
                .Where(x => x.EmployeeId == userid)
                .ToList();
            if (cartItem != null)
            {
                foreach (var i in cartItem)
                    dbcontext.purchaseCarts.Remove(i);
            }

            dbcontext.SaveChanges();
            return RedirectToAction("PurchaseOrder");
        }
        public IActionResult ViewAllPurchaseOrders()
        {
            string userid = HttpContext.Session.GetString("userId");
            Employee user = dbcontext.employees.Where(x => x.Id == userid).FirstOrDefault();
            List<PurchaseOrder> purchaseOrders = dbcontext.purchaseOrders.OrderByDescending(x=>x.date).ToList();
            ViewData["purchaseOrders"] = purchaseOrders;
            ViewData["username"] = user.Name;
            ViewData["user"] = user;
            return View();
        }
        public IActionResult UpdateStatus(string poId)
        {
            PurchaseOrder po = dbcontext.purchaseOrders.Where(x => x.Id == poId).FirstOrDefault();
            po.status = POStatus.DELIVERED;
            dbcontext.Update(po);
            var items = (from c in dbcontext.purchaseOrderDetails
                         where c.PurchaseOrderId == po.Id
                         select new
                         {
                             Inventory = c.InventoryId,
                             Quanity=c.quantity
                         }
                      );
            foreach (var c in items)
            {
                Inventory i = dbcontext.inventories.Where(x => x.Id == c.Inventory).FirstOrDefault();
                i.stock = i.stock + c.Quanity;
                dbcontext.Update(i);
            }
            dbcontext.SaveChanges();
            return RedirectToAction("ViewAllPurchaseOrders");
        }
        public IActionResult PurchaseOrderDetail(string poId)
        {
            List<PurchaseOrderDetails> purchaseOrderDetails = invService.findPurchaseOrderDetails(poId);

            string userid = HttpContext.Session.GetString("userId");
            Employee emp = deptService.findEmployeeById(userid);
            ViewData["purchaseOrderDetail"] = purchaseOrderDetails;
            ViewData["username"] = emp.Name;
            ViewData["userid"] = userid;
            ViewData["user"] = emp;
            PurchaseOrder po = invService.findPurchaseOrder(poId);
            ViewData["po"] = po;
            return View();
        }
    }
}