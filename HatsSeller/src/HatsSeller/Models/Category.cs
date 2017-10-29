using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HatsSeller.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        public String Description { get; set; }

        public ICollection<Hat> Hats { get; set; }
    }
}
