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
        public List<Supplier> getAllSuppliers()
        {
            return dbcontext.suppliers.ToList();
        }

        public List<ItemCategory> retrieveCategories()
        {
            return dbcontext.itemCategories.ToList();
        }
        public ItemCategory retrieveCategory(string id)
        {
            return dbcontext.itemCategories.Where(x => x.Id == id).FirstOrDefault();
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

        public Inventory retrieveInventory(string invId)
        {
            return dbcontext.inventories.Where(x => x.Id == invId).FirstOrDefault();
        }
        public Inventory findInventory(string itemCode)
        {
            return dbcontext.inventories.Where(x => x.itemCode == itemCode).FirstOrDefault();
        }

        public AdjustmentVoucher findAdjustmentVoucher(string id)
        {
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

        public List<PurchaseOrder> findLatestPurchaseOrder()
        {
            List<PurchaseOrder> rq = (from r in dbcontext.purchaseOrders
                                     select r).ToList();
            List<PurchaseOrder> result = new List<PurchaseOrder>();
            for (int i = 0; i < 5; i++)
            {
                result.Add(rq[i]);
            }
            return result;
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
            newAdjustmentVoucher.qty = qty;
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
            if (action == "approve")
            {
                bool res = updateInventory(adjustmentVoucher.InventoryId, adjustmentVoucher.qty);
                switch (res)
                {
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

        public List<AdjustmentVoucher> findAdjustmentVoucherList(Status? status)
        {
            if (status != null)
            {
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
            else
            {
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
        public List<Departments> getAllDepartments()
        {
            return dbcontext.departments.ToList();
        }
        public Departments getDepartmentDetail(string deptId)
        {
            return dbcontext.departments.Where(x => x.Id == deptId).FirstOrDefault();

        }
        public List<String> retrievePurchaseOrder(DateTime dateTime)
        {
            List<String> poList = new List<string>();
            var items = (from c in dbcontext.purchaseOrders
                         where c.date.Month == dateTime.Month
                         select new
                         {
                             poId = c.Id,
                         }
               );
            foreach (var c in items)
            {
                poList.Add(c.poId);

            }
            return poList;
        }
        public List<PurchaseOrderDetails> retrievePurchaseOrderDetails(List<String> poIds)
        {
            List<PurchaseOrderDetails> poDetails = new List<PurchaseOrderDetails>();
            foreach(var c in poIds)
            {
                List<PurchaseOrderDetails> poDetailsList=(dbcontext.purchaseOrderDetails.Where(x => x.PurchaseOrderId == c).ToList());
                foreach (PurchaseOrderDetails d in poDetailsList)
                {
                    poDetails.Add(d);
                }
            }
            return poDetails;
        }
        public Dictionary<string,int> findPurchaseOrderTop(List<PurchaseOrderDetails> poDetails)
        {
            Dictionary<string, int> top3 = new Dictionary<string, int>();
            Dictionary<string, int> top3Result = new Dictionary<string, int>();

            var groupByResult = poDetails.GroupBy(x => x.Inventory.ItemCategory);
            foreach (var group in groupByResult)
            {
                int total = 0;
                string category = "";
                foreach (var detail in group)
                {
                    total += detail.quantity;
                    category = detail.Inventory.ItemCategory.name;
                }
                top3.Add(category, total);
            }
            top3Result = top3.OrderByDescending(x=>x.Value).Take(3).ToDictionary(x=>x.Key,x=>x.Value);
            return top3Result;
        }
        public List<PurchaseOrderQuantity> startPurchaseOrderAnalysis(ItemCategory cat)
        {
            var past4Month = DateTime.Now.AddMonths(-4).Month;
            var Year = DateTime.Now.Year;
            var po = from p in dbcontext.purchaseOrders
                     join pod in dbcontext.purchaseOrderDetails on p.Id equals pod.PurchaseOrderId
                     group pod by new { pod.Inventory.ItemCategory.name, p.date.Month, p.date.Year } into h
                     where (h.Key.Month >= past4Month && h.Key.Year == Year && h.Key.name == cat.name)
                     orderby (h.Key.Month)
                     select new
                     {
                         Month = h.Key.Month,
                         Qty = h.Sum(x => x.quantity)
                     };
            List<PurchaseOrderQuantity> poQ = new List<PurchaseOrderQuantity>();
            foreach (var c in po)
            {
                PurchaseOrderQuantity p = new PurchaseOrderQuantity();
                p.Month = c.Month;
                p.quantity = c.Qty;
                poQ.Add(p);
            }
            return poQ;
        }
    }   
}
