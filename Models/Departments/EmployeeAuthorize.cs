using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Team7_StationeryStore.Models
{
    public class EmployeeAuthorize
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string EmployeeId { get; set; }
        public string DepartmentsId { get; set; }
        public string EmployeeName { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Departments Departments { get; set; }
    }
}
