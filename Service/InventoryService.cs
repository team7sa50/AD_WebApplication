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

        public InventoryService(StationeryContext dbcontext,DepartmentService deptService)
        {
            this.dbcontext = dbcontext;
            this.deptService = deptService;
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
        public void CreatePurchaseOrder(string userId,string supplierId)
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

        public string UpdateAdjustmentVoucher(string id,string action,string remarks) {

            AdjustmentVoucher adjustmentVoucher = findAdjustmentVoucher(id);
            string response = "Adjustment Voucher: "+ adjustmentVoucher.Id;
            if (adjustmentVoucher == null){response += " unable to locate";}
            if (action == "approve") { adjustmentVoucher.status = Status.APPROVED; response+= " approved"; }
            if (action == "reject") { adjustmentVoucher.status = Status.REJECTED; response+= " rejected"; }
            adjustmentVoucher.remarks = remarks;
            dbcontext.Update(adjustmentVoucher);
            dbcontext.SaveChanges();
            return response;
        }


        public void CreateAdjustmentVoucher(string userId, string invId,int qty,string reason) {
            AdjustmentVoucher newAdjustmentVoucher = new AdjustmentVoucher();
            newAdjustmentVoucher.InventoryId = invId;
            newAdjustmentVoucher.EmEmployeeId = userId;
            newAdjustmentVoucher.appEmEmployeeId = setAdjustmentVoucherApprover(userId,invId,qty).Id;
            newAdjustmentVoucher.reason = reason;
            dbcontext.Add(newAdjustmentVoucher);
            dbcontext.SaveChanges();
        }

        public Employee setAdjustmentVoucherApprover(string userId,string invId,int qty) {
            Inventory inventory = retrieveInventory(invId);
            List<Employee> employees = deptService.findDepartmentEmployeeList(userId);
            float dicrepancyCost = inventory.price * qty;
            if (dicrepancyCost < 250)
            {
                return employees.Where(x => x.Role == Role.STORE_SUPERVISOR).FirstOrDefault();
            }
            else { return employees.Where(x => x.Role == Role.STORE_MANAGER).FirstOrDefault(); }
        }


    }
}
