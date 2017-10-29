using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HatsSeller.Models
{
    public class Supplier
    {
        public int SupplierID { get; set; }
        public String SupplierName { get; set; }
        public int HomePhone { get; set; }
        public int WorkPhone { get; set; }
        public int MobilePhone { get; set; }
        public String Email { get; set; }

        public ICollection<Hat> Hats { get; set; }
    }
}
