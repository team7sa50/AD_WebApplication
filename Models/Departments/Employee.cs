using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Team7_StationeryStore.Models
{
    public enum Role { 
        EMPLOYEE,DEPT_HEAD,DEPT_REP,STORE_CLERK,STORE_SUPERVISOR,STORE_MANAGER
    }
    public class Employee
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get;  set; }
        public Role Role { get;  set; }
        [Required]
        public string DepartmentsId { get;  set; }
        public virtual Departments Departments { get;set; }
    }
}
