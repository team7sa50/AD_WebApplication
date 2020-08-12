using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Team7_StationeryStore.Models
{
    public class Supplier
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }
        public string supplierCode { get;  set; }
        public string name { get; set; }
        public string address { get;  set; }
        public int contactNo { get;  set; }
        public int faxNo { get; set; }

    }
}
