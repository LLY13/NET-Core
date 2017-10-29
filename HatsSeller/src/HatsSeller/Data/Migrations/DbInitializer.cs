using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using HatsSeller.Data;

namespace HatsSeller.Models
{
    public static class DbInitializer
    {
        public static void Initialize(HatContext context)
        {
            context.Database.EnsureCreated();
            // Look for any suppliers
            if (context.Suppliers.Any())
             {
                return; // DB has been seeded
            }

            var suppliers = new Supplier[]
            {
                new Supplier {SupplierName="No1 Supplier", WorkPhone=1, MobilePhone=2, HomePhone=3, Email="1supplier@gmail.com" },
                new Supplier {SupplierName="No2 Supplier", WorkPhone=11, MobilePhone=22, HomePhone=33, Email="2supplier@gmail.com" }
            };
            foreach (Supplier s in suppliers)
            {
                context.Suppliers.Add(s);
            }
            context.SaveChanges();

            var categories = new Category[]
            {
                new Category {Description="1" },
                new Category {Description="2" },
                new Category {Description="3" },
                new Category {Description="4" }
            };

            foreach (Category c in categories)
            {
                context.Categories.Add(c);
            }
            context.SaveChanges();

            var hats = new Hat[]
            {
                new Hat {HatName="No1 Hat", Price=1, Description="No1 Hat", SupplierID=1, CategoryID=1,},
                new Hat {HatName="No2 Hat", Price=2, Description="No2 Hat", SupplierID=2, CategoryID=2 }
            };
            foreach (Hat h in hats)
            {
                context.Hats.Add(h);
            }
            context.SaveChanges();

        }



    }
}
