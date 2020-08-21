using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Team7_StationeryStore.Database;
using Team7_StationeryStore.Models;

namespace Team7_StationeryStore.Service
{
    public class InventoryService
    {
        protected StationeryContext dbcontext;
        protected DepartmentService deptService;
        protected NotificationService notificationService;

        public InventoryService(StationeryContext dbcontext, DepartmentService deptService, NotificationService notificationService)
        {
            this.dbcontext = dbcontext;
            this.deptService = deptService;
            this.notificationService = notificationService;
        }
        public List<Inventory> retrieveCatalogue()
        {
            return dbcontext.inventories.ToList();
        }

        public List<Inventory> retrieveCatalogueFromClerk(string suppliers)
        {
            var items = (from c in dbcontext.inventory_Suppliers
                         where c.SupplierId == suppliers
                         select new
                         {
                             Inventory = c.InventoryItemId,
                         }
                      );
            List<Inventory> inventories = new List<Inventory>();
            foreach (var c in items)
            {
                Inventory i = dbcontext.inventories.Where(x => x.Id == c.Inventory).FirstOrDefault();
                inventories.Add(i);

            }
            return inventories;
        }
        public Supplier getSupplier(string supplier)
        {
            return dbcontext.suppliers.Where(x => x.Id == supplier).FirstOrDefault();
        }
        public List<Supplier> getAllSuppliers() {
            return dbcontext.suppliers.ToList();
        }

        public List<ItemCategory> retrieveCategories()
        {
            return dbcontext.itemCategories.ToList();
        }
        public List<PurchaseCart> retrievePurchaseCart(string userId)
        {
            return dbcontext.purchaseCarts.Where(x => x.EmployeeId == userId).ToList();
        }
        public void CreatePurchaseOrder(string userId, string supplierId)
        {
            List<PurchaseCart> cartList = dbcontext.purchaseCarts.Where(x => x.EmployeeId == userId).ToList();
            Employee emp = deptService.findEmployeeById(userId);
            PurchaseOrder newPurchaseOrder = new PurchaseOrder();
            newPurchaseOrder.Id = Guid.NewGuid().ToString();
            newPurchaseOrder.EmployeeId = emp.Id;
            newPurchaseOrder.Employee = emp;
            newPurchaseOrder.status = POStatus.PENDING;
            newPurchaseOrder.date = DateTime.Now;
            newPurchaseOrder.SupplierId = supplierId;
            foreach (var i in cartList)
            {
                Inventory inv = dbcontext.inventories.Where(x => x.Id == i.Id).FirstOrDefault();
                PurchaseOrderDetails purchaseOrderDetail = new PurchaseOrderDetails();
                purchaseOrderDetail.Id = Guid.NewGuid().ToString();
                purchaseOrderDetail.PurchaseOrderId = newPurchaseOrder.Id;
                purchaseOrderDetail.Inventory = i.Inventory;
                purchaseOrderDetail.InventoryId = i.InventoryId;
                purchaseOrderDetail.quantity = i.Qty;
                dbcontext.Add(purchaseOrderDetail);
            }
            dbcontext.Add(newPurchaseOrder);
            dbcontext.purchaseCarts.RemoveRange(cartList);
            dbcontext.SaveChanges();
        }

        public Inventory retrieveInventory(string invId) {
            return dbcontext.inventories.Where(x => x.Id == invId).FirstOrDefault();
        }

        public AdjustmentVoucher findAdjustmentVoucher(string id) {
            return dbcontext.adjustmentVouchers.Where(x => x.Id == id).FirstOrDefault();
        }

        public PurchaseOrder findPurchaseOrder(string poId)
        {
            return dbcontext.purchaseOrders.Where(x => x.Id == poId).FirstOrDefault();
        }
        public List<PurchaseOrderDetails> findPurchaseOrderDetails(string poId)
        {
            return dbcontext.purchaseOrderDetails.Where(x => x.PurchaseOrderId == poId).ToList();
        }

        public void CreateAdjustmentVoucher(string userId, string invId, int qty, string reason) {
            AdjustmentVoucher newAdjustmentVoucher = new AdjustmentVoucher();
            Inventory inventory = retrieveInventory(invId);
            Employee appemployee = setAdjustmentVoucherApprover(userId, invId, qty);
            Employee employee = deptService.findEmployeeById(userId);
            newAdjustmentVoucher.Inventory = inventory;
            newAdjustmentVoucher.InventoryId = invId;
            newAdjustmentVoucher.EmEmployee = employee;
            newAdjustmentVoucher.EmEmployeeId = employee.Id;
            newAdjustmentVoucher.appEmEmployee = appemployee;
            newAdjustmentVoucher.appEmEmployeeId = appemployee.Id;
            newAdjustmentVoucher.reason = reason;
            dbcontext.Add(newAdjustmentVoucher);
            dbcontext.SaveChanges();
            notificationService.sendNotification(NotificationType.ADJUSTMENTVOUCHER, null, null, newAdjustmentVoucher);
        }

        public Employee setAdjustmentVoucherApprover(string userId, string invId, int qty) {
            Inventory inventory = retrieveInventory(invId);
            List<Employee> employees = deptService.findDepartmentEmployeeList(userId);
            double dicrepancyCost = inventory.price * Math.Abs(qty);
            if (dicrepancyCost < 250)
            {
                return employees.Where(x => x.Role == Role.STORE_SUPERVISOR).FirstOrDefault();
            }
            else { return employees.Where(x => x.Role == Role.STORE_MANAGER).FirstOrDefault(); }
        }
        public string UpdateAdjustmentVoucher(string id, string action, string remarks)
        {
            AdjustmentVoucher adjustmentVoucher = findAdjustmentVoucher(id);
            string response = "Adjustment Voucher: [" + adjustmentVoucher.Id + "] request for " + action;
            if (adjustmentVoucher == null) { response += " has failed to locate"; }
            if (action == "approve") {
                bool res = updateInventory(adjustmentVoucher.InventoryId, adjustmentVoucher.qty);
                switch (res) {
                    case true:
                        response += " is sucessed.";
                        adjustmentVoucher.status = Status.APPROVED;
                        break;
                    case false:
                        response += " is denied as there is stock is less than the amount to be deducted.";
                        break;
                }
            }
            if (action == "reject") { adjustmentVoucher.status = Status.REJECTED; response += " is sucessed."; }
            adjustmentVoucher.remarks = remarks;
            dbcontext.Update(adjustmentVoucher);
            dbcontext.SaveChanges();
            return response;
        }

        public List<AdjustmentVoucher> findAdjustmentVoucherList(Status? status) {
            if (status != null) {
                return dbcontext.adjustmentVouchers.Where(x => x.status == status).ToList();
            }
            return dbcontext.adjustmentVouchers.ToList();
        }

        public bool updateInventory(string invId, int qty) {
            bool editable = true;
            Inventory inv = retrieveInventory(invId);
            // To validate if the quantity to be deduct from the stock is sufficient.
            if (qty < 0 && Math.Abs(qty) > inv.stock)
            {
                editable = false;
            }
            else {
                inv.stock += qty;
                dbcontext.Update(inv);
                dbcontext.SaveChanges();
            }
            return editable;
        }
        public List<Inventory> getAllInventories()
        {
            return dbcontext.inventories.ToList();
        }
    } 
}
