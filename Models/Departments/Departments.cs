using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Team7_StationeryStore.Models
{
    public class Departments
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get;  set; }
        [Required]
        public string DeptName { get; set; }
        [Required]
        public string DeptCode { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        [Required]
        public string CollectionPointId { get;  set; }
        [Required]
        public virtual CollectionPoint CollectionPoint { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
