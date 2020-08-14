using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Team7_StationeryStore.Database;
using Team7_StationeryStore.Models;

namespace Team7_StationeryStore.Service
{
    public class InventoryService
    {
        protected StationeryContext dbcontext;

        public InventoryService(StationeryContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public List<Inventory> retrieveCatalogue()
        {
            return dbcontext.inventories.ToList();
        }
    }
}
