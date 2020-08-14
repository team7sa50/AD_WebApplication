using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Team7_StationeryStore.Database;
using Team7_StationeryStore.Models;

namespace Team7_StationeryStore.Controllers
{
    public class PurchaseOrderController : Controller
    {

        protected StationeryContext dbcontext;
        public PurchaseOrderController(StationeryContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult PurchaseOrder()
        {
            List<Supplier> suppliers = dbcontext.suppliers.ToList();
            List<Inventory> inventories = dbcontext.inventories.ToList();
            ViewData["inventories"] = inventories;
            ViewData["suppliers"] = suppliers ;
            return View();
        }
    }
}