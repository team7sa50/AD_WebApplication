using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Team7_StationeryStore.Models;

namespace Team7_StationeryStore.Database
{
    public class StationeryContext : DbContext
    {
        public StationeryContext(DbContextOptions<StationeryContext> options) 
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder model)
        {

        }

        public DbSet<CollectionPoint> collectionPoints { get; set; }
        public DbSet<Departments> departments { get; set; }
        public DbSet<Employee> employees { get; set; }
        public DbSet<EmployeeCart> employeeCarts { get; set; }
        public DbSet<Disbursement> disbursements { get; set; }
        public DbSet<DisbursementDetail> disbursementDetails { get; set; }
        public DbSet<AdjustmentVoucher> adjustmentVouchers { get; set; }
        public DbSet<Inventory> inventories { get; set; }
        public DbSet<Inventory_Supplier> inventory_Suppliers { get; set; }
        public DbSet<ItemCategory> itemCategories { get; set; }
        public DbSet<Supplier> suppliers { get; set; }
        public DbSet<PurchaseOrder> purchaseOrders { get; set; }
        public DbSet<PurchaseOrderDetails> purchaseOrderDetails { get; set; }
        public DbSet<Requisition> requisitions { get; set; }
        public DbSet<RequisitionDetail> requisitionDetails { get; set; }
        public DbSet<Retrieval> retrievals { get; set; }
        public DbSet<RetrievalDetails> retrievalDetails { get; set; }

    }
}
