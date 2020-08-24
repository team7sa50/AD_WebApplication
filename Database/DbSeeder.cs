using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Team7_StationeryStore.Models;

namespace Team7_StationeryStore.Database
{
    public class DbSeeder
    {

        public DbSeeder(StationeryContext dbcontext)
        {
            using (MD5 md5Hash = MD5.Create())
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


                ItemCategory category1 = new ItemCategory();
                category1.Id = Guid.NewGuid().ToString();
                category1.name = "Clip";
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

                Inventory item1 = new Inventory();
                item1.Id = Guid.NewGuid().ToString();
                item1.ItemCategoryId = category1.Id;
                item1.itemCode = "C001";
                item1.description = "Clips Double 1";
                item1.stock = 100;
                item1.measurementUnit = "Dozen";
                item1.location = "bin01";
                item1.reorderLevel = 50;
                item1.reorderQty = 30;
                item1.price = 1;
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
                item2.price = 10;
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
                item3.price = 2;
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
                item4.price = 3;
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
                item5.price = 1.5;
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
                item6.price = 0.5;
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
                item7.price = 0.7;
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
                item8.price = 0.9;
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
                item9.price = 0.8;
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
                item10.price = 1.1;
                dbcontext.Add(item10);

                Inventory item11 = new Inventory();
                item11.Id = Guid.NewGuid().ToString();
                item11.ItemCategoryId = category7.Id;
                item11.itemCode = "P004";
                item11.description = "Pancer Brown(5x7) w/Window";
                item11.stock = 1000;
                item11.measurementUnit = "Each";
                item11.location = "bin07";
                item11.reorderLevel = 600;
                item11.reorderQty = 400;
                item11.price = 1.1;
                dbcontext.Add(item11);

                Inventory item12 = new Inventory();
                item12.Id = Guid.NewGuid().ToString();
                item12.ItemCategoryId = category6.Id;
                item12.itemCode = "PE004";
                item12.description = "Pen red";
                item12.stock = 1000;
                item12.measurementUnit = "Each";
                item12.location = "bin07";
                item12.reorderLevel = 600;
                item12.reorderQty = 400;
                item12.price = 1.1;
                dbcontext.Add(item12);

                Supplier supplier1 = new Supplier();
                supplier1.Id = Guid.NewGuid().ToString();
                supplier1.supplierCode = "ALPA";
                supplier1.name = "ALPHA Office Supplies";
                supplier1.address = "Blk 1128, Ang Mo Kio Industrial Park #02-1108 Ang Mo Kio Street 62 Singapore 622262";
                supplier1.contactNo = 4619928;
                supplier1.faxNo = 4612238;
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

                Inventory_Supplier inventory_Supplier1 = new Inventory_Supplier();
                inventory_Supplier1.Id = Guid.NewGuid().ToString();
                inventory_Supplier1.InventoryItemId = item1.Id;
                inventory_Supplier1.SupplierId = supplier1.Id;
                inventory_Supplier1.qtyOrdered = 300;
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


                Inventory_Supplier inventory_Supplier11 = new Inventory_Supplier();
                inventory_Supplier11.Id = Guid.NewGuid().ToString();
                inventory_Supplier11.InventoryItemId = item11.Id;
                inventory_Supplier11.SupplierId = supplier1.Id;
                inventory_Supplier11.qtyOrdered = 300;
                dbcontext.Add(inventory_Supplier11);

                Inventory_Supplier inventory_Supplier12 = new Inventory_Supplier();
                inventory_Supplier12.Id = Guid.NewGuid().ToString();
                inventory_Supplier12.InventoryItemId = item12.Id;
                inventory_Supplier12.SupplierId = supplier1.Id;
                inventory_Supplier12.qtyOrdered = 300;
                dbcontext.Add(inventory_Supplier12);

                Departments EN = new Departments();
                EN.Id = Guid.NewGuid().ToString();
                EN.DeptCode = "ENGL";
                EN.DeptName = "English Dept";
                EN.ContactName = "English";
                EN.FaxNumber = 0000000;
                EN.PhoneNumber = 1121231231;
                EN.CollectionPointId = cp1.Id;
                dbcontext.Add(EN);

                Departments CS = new Departments();
                CS.Id = Guid.NewGuid().ToString();
                CS.DeptCode = "CPSC";
                CS.DeptName = "ComputerScience";
                CS.ContactName = "Computer";
                CS.FaxNumber = 1111111;
                CS.PhoneNumber = 116561231;
                CS.CollectionPointId = cp3.Id;
                dbcontext.Add(CS);

                Departments Comm = new Departments();
                Comm.Id = Guid.NewGuid().ToString();
                Comm.DeptCode = "ENGL";
                Comm.DeptName = "Commerce Dept";
                Comm.ContactName = "Commerce";
                Comm.FaxNumber = 222222;
                Comm.PhoneNumber = 1121678231;
                Comm.CollectionPointId = cp2.Id;
                dbcontext.Add(Comm);

                Departments regr = new Departments();
                regr.Id = Guid.NewGuid().ToString();
                regr.DeptCode = "Regr";
                regr.DeptName = "Registra Dept";
                regr.ContactName = "Registra";
                regr.FaxNumber = 3333333;
                regr.PhoneNumber = 112129955231;
                regr.CollectionPointId = cp5.Id;
                dbcontext.Add(regr);

                Departments StationeryDept = new Departments();
                StationeryDept.Id = Guid.NewGuid().ToString();
                StationeryDept.DeptCode = "STAT";
                StationeryDept.DeptName = "Stationery Dept";
                StationeryDept.ContactName = "Stationery";
                StationeryDept.PhoneNumber = 65899999;
                StationeryDept.FaxNumber = 444444;
                StationeryDept.CollectionPointId = cp1.Id;
                dbcontext.Add(StationeryDept);

                Employee employee1 = new Employee();
                employee1.Id = Guid.NewGuid().ToString();
                employee1.Name = "nhw";
                employee1.Email = "nhw@gmail.com";
                string emp1psw = MD5Hash.GetMd5Hash(md5Hash, "123");
                employee1.Password = emp1psw;
                employee1.Role = Role.STORE_CLERK;
                employee1.DepartmentsId = StationeryDept.Id;
                dbcontext.Add(employee1);

                Employee employee2 = new Employee();
                employee2.Id = Guid.NewGuid().ToString();
                employee2.Name = "wpa";
                employee2.Email = "wpa@gmail.com";
                string emp2psw = MD5Hash.GetMd5Hash(md5Hash, "123");
                employee2.Password = emp2psw;
                employee2.Role = Role.STORE_SUPERVISOR;
                employee2.DepartmentsId = StationeryDept.Id;
                dbcontext.Add(employee2);

                Employee employee3 = new Employee();
                employee3.Id = Guid.NewGuid().ToString();
                employee3.Name = "keith";
                employee3.Email = "keith@gmail.com";
                string emp3psw = MD5Hash.GetMd5Hash(md5Hash, "123");
                employee3.Password = emp3psw;
                employee3.Role = Role.STORE_MANAGER;
                employee3.DepartmentsId = StationeryDept.Id;
                dbcontext.Add(employee3);
                StationeryDept.DeptHead = employee3.Name;
                StationeryDept.Representative = employee2.Name;

                Employee employee4 = new Employee();
                employee4.Id = Guid.NewGuid().ToString();
                employee4.Name = "tom";
                employee4.Email = "tom@gmail.com";
                string emp4psw = MD5Hash.GetMd5Hash(md5Hash, "123");
                employee4.Password = emp4psw;
                employee4.Role = Role.DEPT_HEAD;
                employee4.DepartmentsId = EN.Id;
                dbcontext.Add(employee4);

                Employee employee5 = new Employee();
                employee5.Id = Guid.NewGuid().ToString();
                employee5.Name = "tessa";
                employee5.Email = "tessa@gmail.com";
                string emp5psw = MD5Hash.GetMd5Hash(md5Hash, "123");
                employee5.Password = emp5psw;
                employee5.Role = Role.DEPT_REP;
                employee5.DepartmentsId = EN.Id;
                dbcontext.Add(employee5);
                EN.Representative = employee5.Name;
                EN.DeptHead = employee4.Name;

                Employee employee6 = new Employee();
                employee6.Id = Guid.NewGuid().ToString();
                employee6.Name = "kk";
                employee6.Email = "kk@gmail.com";
                string emp6psw = MD5Hash.GetMd5Hash(md5Hash, "123");
                employee6.Password = emp6psw;
                employee6.Role = Role.EMPLOYEE;
                employee6.DepartmentsId = EN.Id;
                dbcontext.Add(employee6);


                Employee employee7 = new Employee();
                employee7.Id = Guid.NewGuid().ToString();
                employee7.Name = "marine1";
                employee7.Email = "marine1@gmail.com";
                string emp7psw = MD5Hash.GetMd5Hash(md5Hash, "123");
                employee7.Password = emp7psw;
                employee7.Role = Role.DEPT_HEAD;
                employee7.DepartmentsId = CS.Id;
                dbcontext.Add(employee7);

                Employee employee8 = new Employee();
                employee8.Id = Guid.NewGuid().ToString();
                employee8.Name = "noah";
                employee8.Email = "noah@gmail.com";
                string emp8psw = MD5Hash.GetMd5Hash(md5Hash, "123");
                employee8.Password = emp8psw;
                employee8.Role = Role.DEPT_REP;
                employee8.DepartmentsId = CS.Id;
                dbcontext.Add(employee8);
                CS.DeptHead = employee7.Name;
                CS.Representative = employee8.Name;

                Employee employee9 = new Employee();
                employee9.Id = Guid.NewGuid().ToString();
                employee9.Name = "liam";
                employee9.Email = "liam@gmail.com";
                string emp9psw = MD5Hash.GetMd5Hash(md5Hash, "123");
                employee9.Password = emp9psw;
                employee9.Role = Role.EMPLOYEE;
                employee9.DepartmentsId = CS.Id;
                dbcontext.Add(employee9);


                Employee employee10 = new Employee();
                employee10.Id = Guid.NewGuid().ToString();
                employee10.Name = "emma";
                employee10.Email = "emma@gmail.com";
                string emp10psw = MD5Hash.GetMd5Hash(md5Hash, "123");
                employee10.Password = emp10psw;
                employee10.Role = Role.DEPT_HEAD;
                employee10.DepartmentsId = Comm.Id;
                dbcontext.Add(employee10);

                Employee employee11 = new Employee();
                employee11.Id = Guid.NewGuid().ToString();
                employee11.Name = "willian";
                employee11.Email = "willian@gmail.com";
                string emp11psw = MD5Hash.GetMd5Hash(md5Hash, "123");
                employee11.Password = emp11psw;
                employee11.Role = Role.DEPT_REP;
                employee11.DepartmentsId = Comm.Id;
                dbcontext.Add(employee11);
                Comm.DeptHead = employee10.Name;
                Comm.Representative = employee11.Name;

                Employee employee12 = new Employee();
                employee12.Id = Guid.NewGuid().ToString();
                employee12.Name = "james";
                employee12.Email = "james@gmail.com";
                string emp12psw = MD5Hash.GetMd5Hash(md5Hash, "123");
                employee12.Password = emp12psw;
                employee12.Role = Role.EMPLOYEE;
                employee12.DepartmentsId = Comm.Id;
                dbcontext.Add(employee12);


                Employee employee13 = new Employee();
                employee13.Id = Guid.NewGuid().ToString();
                employee13.Name = "ava";
                employee13.Email = "ava@gmail.com";
                string emp13psw = MD5Hash.GetMd5Hash(md5Hash, "123");
                employee13.Password = emp13psw;
                employee13.Role = Role.DEPT_HEAD;
                employee13.DepartmentsId = regr.Id;
                dbcontext.Add(employee13);

                Employee employee14 = new Employee();
                employee14.Id = Guid.NewGuid().ToString();
                employee14.Name = "isabella";
                employee14.Email = "isabella@gmail.com";
                string emp14psw = MD5Hash.GetMd5Hash(md5Hash, "123");
                employee14.Password = emp14psw;
                employee14.Role = Role.DEPT_REP;
                employee14.DepartmentsId = regr.Id;
                dbcontext.Add(employee14);
                regr.DeptHead = employee13.Name;
                regr.Representative = employee14.Name;

                Employee employee15 = new Employee();
                employee15.Id = Guid.NewGuid().ToString();
                employee15.Name = "liam";
                employee15.Email = "liam@gmail.com";
                string emp15psw = MD5Hash.GetMd5Hash(md5Hash, "123");
                employee15.Password = emp15psw;
                employee15.Role = Role.EMPLOYEE;
                employee15.DepartmentsId = regr.Id;
                dbcontext.Add(employee15);

                PurchaseOrder po1 = new PurchaseOrder();
                po1.Id = Guid.NewGuid().ToString();
                po1.SupplierId = supplier1.Id;
                po1.EmployeeId = employee1.Id;
                po1.date = DateTime.Now;
                po1.status = POStatus.PENDING;
                dbcontext.Add(po1);

                PurchaseOrderDetails po1detail1 = new PurchaseOrderDetails();
                po1detail1.Id = Guid.NewGuid().ToString();
                po1detail1.PurchaseOrderId = po1.Id;
                po1detail1.InventoryId = item1.Id;
                po1detail1.quantity = 20;
                dbcontext.Add(po1detail1);

                PurchaseOrderDetails po1detail2 = new PurchaseOrderDetails();
                po1detail2.Id = Guid.NewGuid().ToString();
                po1detail2.PurchaseOrderId = po1.Id;
                po1detail2.InventoryId = item3.Id;
                po1detail2.quantity = 20;
                dbcontext.Add(po1detail2);

                PurchaseOrderDetails po1detail3 = new PurchaseOrderDetails();
                po1detail3.Id = Guid.NewGuid().ToString();
                po1detail3.PurchaseOrderId = po1.Id;
                po1detail3.InventoryId = item4.Id;
                po1detail3.quantity = 20;
                dbcontext.Add(po1detail3);

                PurchaseOrder po2 = new PurchaseOrder();
                po2.Id = Guid.NewGuid().ToString();
                po2.SupplierId = supplier2.Id;
                po2.EmployeeId = employee1.Id;
                po2.date = DateTime.Now.AddMonths(-1);
                po2.status = POStatus.PENDING;
                dbcontext.Add(po2);

                PurchaseOrderDetails po2detail1 = new PurchaseOrderDetails();
                po2detail1.Id = Guid.NewGuid().ToString();
                po2detail1.PurchaseOrderId = po2.Id;
                po2detail1.InventoryId = item2.Id;
                po2detail1.quantity = 20;
                dbcontext.Add(po2detail1);

                PurchaseOrderDetails po2detail2 = new PurchaseOrderDetails();
                po2detail2.Id = Guid.NewGuid().ToString();
                po2detail2.PurchaseOrderId = po2.Id;
                po2detail2.InventoryId = item7.Id;
                po2detail2.quantity = 20;
                dbcontext.Add(po2detail2);

                PurchaseOrderDetails po2detail3 = new PurchaseOrderDetails();
                po2detail3.Id = Guid.NewGuid().ToString();
                po2detail3.PurchaseOrderId = po2.Id;
                po2detail3.InventoryId = item5.Id;
                po2detail3.quantity = 20;
                dbcontext.Add(po2detail3);

                PurchaseOrderDetails po2detail4 = new PurchaseOrderDetails();
                po2detail4.Id = Guid.NewGuid().ToString();
                po2detail4.PurchaseOrderId = po2.Id;
                po2detail4.InventoryId = item11.Id;
                po2detail4.quantity = 10;
                dbcontext.Add(po2detail4);

                PurchaseOrderDetails po2detail5 = new PurchaseOrderDetails();
                po2detail5.Id = Guid.NewGuid().ToString();
                po2detail5.PurchaseOrderId = po2.Id;
                po2detail5.InventoryId = item12.Id;
                po2detail5.quantity = 15;
                dbcontext.Add(po2detail5);


                Requisition requisition1 = new Requisition();
                requisition1.Id = EN.DeptCode + "_" + DateTime.Now.ToString("MM/dd/yyyy/HH:mm:ss");
                requisition1.DepartmentId = EN.Id;
                requisition1.EmployeeId = employee6.Id;
                requisition1.ApprovedEmployeeId = employee4.Id;
                requisition1.DateSubmitted = DateTime.Now;
                requisition1.status = ReqStatus.APPROVED;
                requisition1.Remarks = "nothing";
                dbcontext.Add(requisition1);

                RequisitionDetail requisition1Detail = new RequisitionDetail();
                requisition1Detail.Id = Guid.NewGuid().ToString();
                requisition1Detail.RequisitionId = requisition1.Id;
                requisition1Detail.InventoryId = item1.Id;
                requisition1Detail.RequestedQty = 20;
                requisition1Detail.DistributedQty = 0;
                dbcontext.Add(requisition1Detail);

                RequisitionDetail requisition1Detail2 = new RequisitionDetail();
                requisition1Detail2.Id = Guid.NewGuid().ToString();
                requisition1Detail2.RequisitionId = requisition1.Id;
                requisition1Detail2.InventoryId = item2.Id;
                requisition1Detail2.RequestedQty = 20;
                requisition1Detail2.DistributedQty = 0;
                dbcontext.Add(requisition1Detail2);

                Requisition requisition2 = new Requisition();
                requisition2.Id = CS.DeptCode + "_" + DateTime.Now.ToString("MM/dd/yyyy/HH:mm:ss");
                requisition2.DepartmentId = CS.Id;
                requisition2.EmployeeId = employee9.Id;
                requisition2.ApprovedEmployeeId = employee7.Id;
                requisition2.DateSubmitted = DateTime.Now;
                requisition2.status = ReqStatus.OUTSTAND;
                requisition2.Remarks = "nothing";
                dbcontext.Add(requisition2);

                RequisitionDetail requisition2Detail = new RequisitionDetail();
                requisition2Detail.Id = Guid.NewGuid().ToString();
                requisition2Detail.RequisitionId = requisition2.Id;
                requisition2Detail.InventoryId = item2.Id;
                requisition2Detail.RequestedQty = 20;
                requisition2Detail.DistributedQty = 0;
                dbcontext.Add(requisition2Detail);

                RequisitionDetail requisition2Detail2 = new RequisitionDetail();
                requisition2Detail2.Id = Guid.NewGuid().ToString();
                requisition2Detail2.RequisitionId = requisition2.Id;
                requisition2Detail2.InventoryId = item3.Id;
                requisition2Detail2.RequestedQty = 20;
                requisition2Detail2.DistributedQty = 0;
                dbcontext.Add(requisition2Detail2);

                //-------------------------------------------------------------Seeding for training model-------------------------------------------------------------------------------------->
                Inventory[] arrInv = new Inventory[] { item1, item2, item3, item4, item5, item6, item7, item8, item9, item10 };

                Random rand = new Random();
                int size = 300;
                for (int i = 0; i < size; i++)
                {
                    DateTime randDate = DateTime.Now.AddDays(-rand.Next(1100));
                    int randomQty = rand.Next(1, 100);
                    int randomStatus = rand.Next(0, 7);
                    Requisition r1 = new Requisition();
                    r1.Id = EN.DeptCode + "_" + Guid.NewGuid().ToString();
                    r1.DepartmentId = EN.Id;
                    r1.EmployeeId = employee6.Id;
                    r1.ApprovedEmployeeId = employee4.Id;
                    r1.DateSubmitted = randDate;
                    r1.status = (ReqStatus)randomStatus;
                    r1.Remarks = "nothing";
                    List<string> list = new List<string>();
                    List<RequisitionDetail> rLists = new List<RequisitionDetail>();
                    for (int j = 0; j < 3; j++) {
                        RequisitionDetail rd1 = new RequisitionDetail();
                        rd1.Id = Guid.NewGuid().ToString();
                        int randomItem = rand.Next(arrInv.Length);
                        string item= arrInv[randomItem].Id;
                        while (list.FirstOrDefault(x => x.Contains(item)) != null)
                        {
                            randomItem = rand.Next(arrInv.Length);
                            item = arrInv[randomItem].Id;
                        }
                        rd1.RequisitionId = r1.Id;
                        rd1.Inventory = arrInv[randomItem];
                        //rd1.InventoryId = item;
                        rd1.RequestedQty = randomQty;
                        rd1.DistributedQty = 0;
                        list.Add(arrInv[randomItem].Id);
                        rLists.Add(rd1);
                    }
                    list.Clear();
                    dbcontext.AddRangeAsync(rLists);
                    dbcontext.Add(r1);

                    int randomPOStatus = rand.Next(0, 2);
                    PurchaseOrder po11 = new PurchaseOrder();
                    po11.Id = Guid.NewGuid().ToString();
                    po11.SupplierId = supplier1.Id;
                    po11.EmployeeId = employee1.Id;
                    po11.date = randDate;
                    po11.status = (POStatus)randomPOStatus;
                    dbcontext.Add(po11);
                    for (int k = 0; k < 5; k++)
                    {
                        int randomI = rand.Next(arrInv.Length);
                        PurchaseOrderDetails pod1 = new PurchaseOrderDetails();
                        pod1.Id = Guid.NewGuid().ToString();
                        pod1.PurchaseOrderId = po11.Id;
                        pod1.InventoryId = arrInv[randomI].Id;
                        pod1.quantity = randomQty;
                        dbcontext.Add(pod1);
                    }
                }

                //-------------------------------------------------------------Seeding for training model-------------------------------------------------------------------------------------->

                Requisition requisition3 = new Requisition();
                requisition3.Id = regr.DeptCode + "_" + DateTime.Now.ToString("MM/dd/yyyy/HH:mm:ss");
                requisition3.DepartmentId = regr.Id;
                requisition3.EmployeeId = employee15.Id;
                requisition3.ApprovedEmployeeId = employee13.Id;
                requisition3.DateSubmitted = DateTime.Now;
                requisition3.status = ReqStatus.REJECTED;
                requisition3.Remarks = "nothing";
                dbcontext.Add(requisition3);

                Requisition requisition4 = new Requisition();
                requisition4.Id = regr.DeptCode + "/" + DateTime.Now;
                requisition4.DepartmentId = regr.Id;
                requisition4.EmployeeId = employee15.Id;
                requisition4.ApprovedEmployeeId = employee13.Id;
                requisition4.DateSubmitted = DateTime.Now;
                requisition4.status = ReqStatus.PROCESSING;
                requisition4.Remarks = "nothing";
                dbcontext.Add(requisition4);


                Disbursement d1 = new Disbursement()
                {
                    Id = Guid.NewGuid().ToString(),
                    GeneratedDate = DateTime.Now,
                    CollectionDate = DateTime.Now,
                    status = DisbusementStatus.COMPLETED,
                    Departments = regr
                };
                dbcontext.Add(d1);

                Disbursement d2 = new Disbursement()
                {
                    Id = Guid.NewGuid().ToString(),
                    GeneratedDate = DateTime.Now,
                    CollectionDate = DateTime.Now,
                    status = DisbusementStatus.COMPLETED,
                    Departments = Comm
                };
                dbcontext.Add(d2);

                Disbursement d3 = new Disbursement()
                {
                    Id = Guid.NewGuid().ToString(),
                    GeneratedDate = DateTime.Now,
                    CollectionDate = DateTime.Now,
                    status = DisbusementStatus.COMPLETED,
                    Departments = CS
                };
                dbcontext.Add(d3);

                Disbursement d4 = new Disbursement()
                {
                    Id = Guid.NewGuid().ToString(),
                    GeneratedDate = DateTime.Now,
                    CollectionDate = DateTime.Now,
                    status = DisbusementStatus.COMPLETED,
                    Departments = EN
                };
                dbcontext.Add(d4);

                Disbursement d5 = new Disbursement()
                {
                    Id = Guid.NewGuid().ToString(),
                    GeneratedDate = DateTime.Now,
                    CollectionDate = DateTime.Now,
                    status = DisbusementStatus.COMPLETED,
                    Departments = Comm
                };
                dbcontext.Add(d5);

                dbcontext.SaveChanges();

            }
        }
    }
}
