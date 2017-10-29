using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace HatsSeller.Models
{
    public class Hat
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int HatID { get; set; }
        public int SupplierID { get; set; }
        public int CategoryID { get; set; }
        public String HatName { get; set; }
        public Decimal Price { get; set; }
        public String Description { get; set; }
        //image
        public string PathOfFile { get; set; }

        //foreign key
        public Supplier Supplier { get; set; }
        public Category Category { get; set; }
    }
}
