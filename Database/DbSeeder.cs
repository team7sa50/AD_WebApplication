using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Team7_StationeryStore.Models;

namespace Team7_StationeryStore.Database
{
    public class DbSeeder
    {
        public DbSeeder(StationeryContext dbcontext)
        {
            string time1 = "9:30am";
            string time2 = "11:00am";
            CollectionPoint cp1 = new CollectionPoint();
            cp1.Id = Guid.NewGuid().ToString();
            cp1.Location = "Stationery Store - Administration Building";
            cp1.Time = time1;
            dbcontext.Add(cp1);

            CollectionPoint cp2 = new CollectionPoint();
            cp2.Id = Guid.NewGuid().ToString();
            cp2.Location = "Management School";
            cp2.Time = time2;
            dbcontext.Add(cp2);

            CollectionPoint cp3 = new CollectionPoint();
            cp3.Id = Guid.NewGuid().ToString();
            cp3.Location = "Medical School";
            cp3.Time = time1;
            dbcontext.Add(cp3);

            CollectionPoint cp4 = new CollectionPoint();
            cp4.Id = Guid.NewGuid().ToString();
            cp4.Location = "Engineering School";
            cp4.Time = time1;
            dbcontext.Add(cp4);

            CollectionPoint cp5 = new CollectionPoint();
            cp5.Id = Guid.NewGuid().ToString();
            cp5.Location = "Science School";
            cp5.Time = time1;
            dbcontext.Add(cp5);

            CollectionPoint cp6 = new CollectionPoint();
            cp6.Id = Guid.NewGuid().ToString();
            cp6.Location = "University Hospital";
            cp6.Time = time2;
            dbcontext.Add(cp6);


            ItemCategory category1  =  new ItemCategory();
            category1.Id= Guid.NewGuid().ToString();
            category1.name  =  "Clip";
            dbcontext.Add(category1);

            ItemCategory category2 = new ItemCategory();
            category2.Id = Guid.NewGuid().ToString();
            category2.name = "Envelope";
            dbcontext.Add(category2);

            ItemCategory category3 = new ItemCategory();
            category3.Id = Guid.NewGuid().ToString();
            category3.name = "Eraser";
            dbcontext.Add(category3);

            ItemCategory category4 = new ItemCategory();
            category4.Id = Guid.NewGuid().ToString();
            category4.name = "Exercise";
            dbcontext.Add(category4);

            ItemCategory category5 = new ItemCategory();
            category5.Id = Guid.NewGuid().ToString();
            category5.name = "File";
            dbcontext.Add(category5);

            ItemCategory category6 = new ItemCategory();
            category6.Id = Guid.NewGuid().ToString();
            category6.name = "Pen";
            dbcontext.Add(category6);

            ItemCategory category7 = new ItemCategory();
            category7.Id = Guid.NewGuid().ToString();
            category7.name = "Puncher";
            dbcontext.Add(category7);

            ItemCategory category8 = new ItemCategory();
            category8.Id = Guid.NewGuid().ToString();
            category8.name = "Pad";
            dbcontext.Add(category8);

            ItemCategory category9 = new ItemCategory();
            category9.Id = Guid.NewGuid().ToString();
            category9.name = "Paper";
            dbcontext.Add(category9);

            ItemCategory category10 = new ItemCategory();
            category10.Id = Guid.NewGuid().ToString();
            category10.name = "Ruler";
            dbcontext.Add(category10);

            Inventory item1  =  new Inventory();
            item1.Id= Guid.NewGuid().ToString();
            item1.ItemCategoryId  =  category1.Id;
            item1.itemCode = "C001";
            item1.description = "Clips Double 1";
            item1.stock  =  100;
            item1.measurementUnit  =  "Dozen";
            item1.location  =  "bin01";
            item1.reorderLevel  =  50;
            item1.reorderQty  =  30;
            dbcontext.Add(item1);

            Inventory item2 = new Inventory();
            item2.Id = Guid.NewGuid().ToString();
            item2.ItemCategoryId = category1.Id;
            item2.itemCode = "C002";
            item2.description = "Clips Double 2";
            item2.stock = 100;
            item2.measurementUnit = "Dozen";
            item2.location = "bin01";
            item2.reorderLevel = 50;
            item2.reorderQty = 30;
            dbcontext.Add(item2);

            Inventory item3 = new Inventory();
            item3.Id = Guid.NewGuid().ToString();
            item3.ItemCategoryId = category1.Id;
            item3.itemCode = "C003";
            item3.description = "Clips Double 3/4";
            item3.stock = 100;
            item3.measurementUnit = "Dozen";
            item3.location = "bin01";
            item3.reorderLevel = 50;
            item3.reorderQty = 30;
            dbcontext.Add(item3);

            Inventory item4 = new Inventory();
            item4.Id = Guid.NewGuid().ToString();
            item4.ItemCategoryId = category1.Id;
            item4.itemCode = "C004";
            item4.description = "Clips Paper Large";
            item4.stock = 100;
            item4.measurementUnit = "Box";
            item4.location = "bin01";
            item4.reorderLevel = 50;
            item4.reorderQty = 30;
            dbcontext.Add(item4);

            Inventory item5 = new Inventory();
            item5.Id = Guid.NewGuid().ToString();
            item5.ItemCategoryId = category1.Id;
            item5.itemCode = "C005";
            item5.description = "Clips Paper Medium";
            item5.stock = 100;
            item5.measurementUnit = "Box";
            item5.location = "bin01";
            item5.reorderLevel = 50;
            item5.reorderQty = 30;
            dbcontext.Add(item5);

            Inventory item6 = new Inventory();
            item6.Id = Guid.NewGuid().ToString();
            item6.ItemCategoryId = category1.Id;
            item6.itemCode = "C006";
            item6.description = "Clips Paper Small";
            item6.stock = 100;
            item6.measurementUnit = "Box";
            item6.location = "bin01";
            item6.reorderLevel = 50;
            item6.reorderQty = 30;
            dbcontext.Add(item6);

            Inventory item7 = new Inventory();
            item7.Id = Guid.NewGuid().ToString();
            item7.ItemCategoryId = category2.Id;
            item7.itemCode = "E001";
            item7.description = "Envelope Brown(3x6)";
            item7.stock = 1000;
            item7.measurementUnit = "Each";
            item7.location = "bin02";
            item7.reorderLevel = 600;
            item7.reorderQty = 400;
            dbcontext.Add(item7);

            Inventory item8 = new Inventory();
            item8.Id = Guid.NewGuid().ToString();
            item8.ItemCategoryId = category2.Id;
            item8.itemCode = "E002";
            item8.description = "Envelope Brown(3x6) w/Window";
            item8.stock = 1000;
            item8.measurementUnit = "Each";
            item8.location = "bin02";
            item8.reorderLevel = 600;
            item8.reorderQty = 400;
            dbcontext.Add(item8);

            Inventory item9 = new Inventory();
            item9.Id = Guid.NewGuid().ToString();
            item9.ItemCategoryId = category2.Id;
            item9.itemCode = "E003";
            item9.description = "Envelope Brown(5x7)";
            item9.stock = 1000;
            item9.measurementUnit = "Each";
            item9.location = "bin02";
            item9.reorderLevel = 600;
            item9.reorderQty = 400;
            dbcontext.Add(item9);

            Inventory item10 = new Inventory();
            item10.Id = Guid.NewGuid().ToString();
            item10.ItemCategoryId = category2.Id;
            item10.itemCode = "E004";
            item10.description = "Envelope Brown(5x7) w/Window";
            item10.stock = 1000;
            item10.measurementUnit = "Each";
            item10.location = "bin02";
            item10.reorderLevel = 600;
            item10.reorderQty = 400;
            dbcontext.Add(item10);

            Supplier supplier1  =  new Supplier();
            supplier1.Id= Guid.NewGuid().ToString();
            supplier1.supplierCode  =  "ALPA";
            supplier1.name  =  "ALPHA Office Supplies";
            supplier1.address  = "Blk 1128, Ang Mo Kio Industrial Park #02-1108 Ang Mo Kio Street 62 Singapore 622262";
            supplier1.contactNo  = 4619928;
            supplier1.faxNo= 4612238;
            dbcontext.Add(supplier1);

            Supplier supplier2 = new Supplier();
            supplier2.Id = Guid.NewGuid().ToString();
            supplier2.supplierCode = "CHEP";
            supplier2.name = "Cheap Stationer";
            supplier2.address = "Blk 34, Clementi Road #07-02 Ban Ban Soh Building Singapore 110525";
            supplier2.contactNo = 3543234;
            supplier2.faxNo = 4742434;
            dbcontext.Add(supplier2);

            Supplier supplier3 = new Supplier();
            supplier3.Id = Guid.NewGuid().ToString();
            supplier3.supplierCode = "BANE";
            supplier3.name = "BANES Shop";
            supplier3.address = "Blk 124, Alexandra Road #03-04 Banes Building Singapore 550315";
            supplier3.contactNo = 4781234;
            supplier3.faxNo = 4792434;
            dbcontext.Add(supplier3);

            Inventory_Supplier inventory_Supplier1  =  new Inventory_Supplier();
            inventory_Supplier1.Id= Guid.NewGuid().ToString();
            inventory_Supplier1.InventoryItemId  =  item1.Id;
            inventory_Supplier1.SupplierId = supplier1.Id;
            inventory_Supplier1.qtyOrdered  =  300;
            dbcontext.Add(inventory_Supplier1);

            Inventory_Supplier inventory_Supplier2 = new Inventory_Supplier();
            inventory_Supplier2.Id = Guid.NewGuid().ToString();
            inventory_Supplier2.InventoryItemId = item2.Id;
            inventory_Supplier2.SupplierId = supplier1.Id;
            inventory_Supplier2.qtyOrdered = 300;
            dbcontext.Add(inventory_Supplier2);

            Inventory_Supplier inventory_Supplier3 = new Inventory_Supplier();
            inventory_Supplier3.Id = Guid.NewGuid().ToString();
            inventory_Supplier3.InventoryItemId = item3.Id;
            inventory_Supplier3.SupplierId = supplier1.Id;
            inventory_Supplier3.qtyOrdered = 300;
            dbcontext.Add(inventory_Supplier3);

            Inventory_Supplier inventory_Supplier4 = new Inventory_Supplier();
            inventory_Supplier4.Id = Guid.NewGuid().ToString();
            inventory_Supplier4.InventoryItemId = item4.Id;
            inventory_Supplier4.SupplierId = supplier1.Id;
            inventory_Supplier4.qtyOrdered = 300;
            dbcontext.Add(inventory_Supplier4);

            Inventory_Supplier inventory_Supplier5 = new Inventory_Supplier();
            inventory_Supplier5.Id = Guid.NewGuid().ToString();
            inventory_Supplier5.InventoryItemId = item5.Id;
            inventory_Supplier5.SupplierId = supplier2.Id;
            inventory_Supplier5.qtyOrdered = 300;
            dbcontext.Add(inventory_Supplier5);

            Inventory_Supplier inventory_Supplier6 = new Inventory_Supplier();
            inventory_Supplier6.Id = Guid.NewGuid().ToString();
            inventory_Supplier6.InventoryItemId = item6.Id;
            inventory_Supplier6.SupplierId = supplier2.Id;
            inventory_Supplier6.qtyOrdered = 300;
            dbcontext.Add(inventory_Supplier6);

            Inventory_Supplier inventory_Supplier7 = new Inventory_Supplier();
            inventory_Supplier7.Id = Guid.NewGuid().ToString();
            inventory_Supplier7.InventoryItemId = item7.Id;
            inventory_Supplier7.SupplierId = supplier2.Id;
            inventory_Supplier7.qtyOrdered = 300;
            dbcontext.Add(inventory_Supplier7);

            Inventory_Supplier inventory_Supplier8 = new Inventory_Supplier();
            inventory_Supplier8.Id = Guid.NewGuid().ToString();
            inventory_Supplier8.InventoryItemId = item8.Id;
            inventory_Supplier8.SupplierId = supplier3.Id;
            inventory_Supplier8.qtyOrdered = 300;
            dbcontext.Add(inventory_Supplier8);

            Inventory_Supplier inventory_Supplier9 = new Inventory_Supplier();
            inventory_Supplier9.Id = Guid.NewGuid().ToString();
            inventory_Supplier9.InventoryItemId = item9.Id;
            inventory_Supplier9.SupplierId = supplier3.Id;
            inventory_Supplier9.qtyOrdered = 300;
            dbcontext.Add(inventory_Supplier9);

            Inventory_Supplier inventory_Supplier10 = new Inventory_Supplier();
            inventory_Supplier10.Id = Guid.NewGuid().ToString();
            inventory_Supplier10.InventoryItemId = item10.Id;
            inventory_Supplier10.SupplierId = supplier3.Id;
            inventory_Supplier10.qtyOrdered = 300;
            dbcontext.Add(inventory_Supplier10);




            Departments EN = new Departments();
            EN.Id = Guid.NewGuid().ToString();
            EN.DeptCode = "ENGL";
            EN.DeptName = "English Dept";
            EN.DeptHead = "marine";
            EN.FaxNumber = 0000000;
            EN.PhoneNumber = 1121231231;
            EN.CollectionPointId = cp1.Id;
            dbcontext.Add(EN);

            Departments CS = new Departments();
            CS.Id = Guid.NewGuid().ToString();
            CS.DeptCode = "CPSC";
            CS.DeptName = "ComputerScience";
            CS.DeptHead = "tom";
            CS.FaxNumber = 1111111;
            CS.PhoneNumber = 116561231;
            CS.CollectionPointId = cp3.Id;
            dbcontext.Add(CS);

            Departments Comm = new Departments();
            Comm.Id = Guid.NewGuid().ToString();
            Comm.DeptCode = "ENGL";
            Comm.DeptName = "Commerce Dept";
            Comm.DeptHead = "emma";
            Comm.FaxNumber = 222222;
            Comm.PhoneNumber = 1121678231;
            Comm.CollectionPointId = cp2.Id;
            dbcontext.Add(Comm);

            Departments regr = new Departments();
            regr.Id = Guid.NewGuid().ToString();
            regr.DeptCode = "ENGL";
            regr.DeptName = "Registra Dept";
            regr.DeptHead = "ava";
            regr.FaxNumber = 3333333;
            regr.PhoneNumber = 112129955231;
            regr.CollectionPointId = cp5.Id;
            dbcontext.Add(regr);

            Departments StationeryDept = new Departments();
            StationeryDept.Id = Guid.NewGuid().ToString();
            StationeryDept.DeptCode = "STAT";
            StationeryDept.DeptName = "Stationery Dept";
            StationeryDept.DeptHead = "marine";
            StationeryDept.PhoneNumber = 65899999;
            StationeryDept.FaxNumber = 444444;
            StationeryDept.CollectionPointId = cp1.Id;
            dbcontext.Add(StationeryDept);

            Employee employee1  =  new Employee();
            employee1.Id  =  Guid.NewGuid().ToString();
            employee1.Name  =  "nhw";
            employee1.Email  =  "nhw@gmail.com";
            employee1.Password  =  "123";
            employee1.Role  =  Role.STORE_CLERK;
            employee1.DepartmentsId = StationeryDept.Id;
            dbcontext.Add(employee1);

            Employee employee2 = new Employee();
            employee2.Id = Guid.NewGuid().ToString();
            employee2.Name = "wpa";
            employee2.Email = "wpa@gmail.com";
            employee2.Password = "123";
            employee2.Role = Role.STORE_SUPERVISOR;
            employee2.DepartmentsId = StationeryDept.Id;
            dbcontext.Add(employee2);

            Employee employee3 = new Employee();
            employee3.Id = Guid.NewGuid().ToString();
            employee3.Name = "keith";
            employee3.Email = "keith@gmail.com";
            employee3.Password = "123";
            employee3.Role = Role.STORE_MANAGER;
            employee3.DepartmentsId = StationeryDept.Id;
            dbcontext.Add(employee3);

            Employee employee4 = new Employee();
            employee4.Id = Guid.NewGuid().ToString();
            employee4.Name = "tom";
            employee4.Email = "tom@gmail.com";
            employee4.Password = "123";
            employee4.Role = Role.DEPT_HEAD;
            employee4.DepartmentsId = EN.Id  ;
            dbcontext.Add(employee4);

            Employee employee5 = new Employee();
            employee5.Id = Guid.NewGuid().ToString();
            employee5.Name = "tessa";
            employee5.Email = "tessa@gmail.com";
            employee5.Password = "123";
            employee5.Role = Role.DEPT_REP;
            employee5.DepartmentsId = EN.Id ;
            dbcontext.Add(employee5);

            Employee employee6 = new Employee();
            employee6.Id = Guid.NewGuid().ToString();
            employee6.Name = "kk";
            employee6.Email = "kk@gmail.com";
            employee6.Password = "123";
            employee6.Role = Role.EMPLOYEE;
            employee6.DepartmentsId = EN.Id  ;
            dbcontext.Add(employee6);


            Employee employee7 = new Employee();
            employee7.Id = Guid.NewGuid().ToString();
            employee7.Name = "marine1";
            employee7.Email = "marine1@gmail.com";
            employee7.Password = "123";
            employee7.Role = Role.DEPT_HEAD;
            employee7.DepartmentsId = CS.Id  ;
            dbcontext.Add(employee7);

            Employee employee8 = new Employee();
            employee8.Id = Guid.NewGuid().ToString();
            employee8.Name = "noah";
            employee8.Email = "noah@gmail.com";
            employee8.Password = "123";
            employee8.Role = Role.DEPT_REP;
            employee8.DepartmentsId = CS.Id  ;
            dbcontext.Add(employee8);

            Employee employee9 = new Employee();
            employee9.Id = Guid.NewGuid().ToString();
            employee9.Name = "liam";
            employee9.Email = "liam@gmail.com";
            employee9.Password = "123";
            employee9.Role = Role.EMPLOYEE;
            employee9.DepartmentsId = CS.Id  ;
            dbcontext.Add(employee9);


            Employee employee10 = new Employee();
            employee10.Id = Guid.NewGuid().ToString();
            employee10.Name = "emma";
            employee10.Email = "emma@gmail.com";
            employee10.Password = "123";
            employee10.Role = Role.DEPT_HEAD;
            employee10.DepartmentsId = Comm.Id;
            dbcontext.Add(employee10);

            Employee employee11 = new Employee();
            employee11.Id = Guid.NewGuid().ToString();
            employee11.Name = "willian";
            employee11.Email = "willian@gmail.com";
            employee11.Password = "123";
            employee11.Role = Role.DEPT_REP;
            employee11.DepartmentsId = Comm.Id;
            dbcontext.Add(employee11);

            Employee employee12 = new Employee();
            employee12.Id = Guid.NewGuid().ToString();
            employee12.Name = "james";
            employee12.Email = "james@gmail.com";
            employee12.Password = "123";
            employee12.Role = Role.EMPLOYEE;
            employee12.DepartmentsId = Comm.Id;
            dbcontext.Add(employee12);


            Employee employee13 = new Employee();
            employee13.Id = Guid.NewGuid().ToString();
            employee13.Name = "ava";
            employee13.Email = "ava@gmail.com";
            employee13.Password = "123";
            employee13.Role = Role.DEPT_HEAD;
            employee13.DepartmentsId = regr.Id;
            dbcontext.Add(employee13);

            Employee employee14 = new Employee();
            employee14.Id = Guid.NewGuid().ToString();
            employee14.Name = "isabella";
            employee14.Email = "isabella@gmail.com";
            employee14.Password = "123";
            employee14.Role = Role.DEPT_REP;
            employee14.DepartmentsId = regr.Id;
            dbcontext.Add(employee14);

            Employee employee15 = new Employee();
            employee15.Id = Guid.NewGuid().ToString();
            employee15.Name = "liam";
            employee15.Email = "liam@gmail.com";
            employee15.Password = "123";
            employee15.Role = Role.EMPLOYEE;
            employee15.DepartmentsId = regr.Id;
            dbcontext.Add(employee15);
            dbcontext.SaveChanges();



        }
    }
}
