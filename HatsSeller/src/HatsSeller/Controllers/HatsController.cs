using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HatsSeller.Data;
using HatsSeller.Models;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers; //Week 6
using Microsoft.AspNetCore.Hosting; //Week 6
using Microsoft.AspNetCore.Http; //Week 6
using System.IO; //Week 6


namespace HatsSeller.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HatsController : Controller
    {
        private readonly HatContext _context;
        private readonly IHostingEnvironment _hostingEnv;

        public HatsController(HatContext context, IHostingEnvironment hEnv)
        {
            _context = context;
            _hostingEnv = hEnv;
        }

        // GET: Hats
        public async Task<IActionResult> Index(
string sortOrder,
string currentFilter,
string searchString,
int? page)
        {

            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;

            var hats = from s in _context.Hats
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                hats = hats.Where(s => s.HatName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    hats = hats.OrderByDescending(s => s.HatName);
                    break;
                default:
                    hats = hats.OrderBy(s => s.HatName);
                    break;
            }
            int pageSize = 3;
            return View(await PaginatedList<Hat>.CreateAsync(hats.AsNoTracking(), page ?? 1, pageSize));
        }

        // GET: Hats/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hat = await _context.Hats.SingleOrDefaultAsync(m => m.HatID == id);
            if (hat == null)
            {
                return NotFound();
            }

            return View(hat);
        }

        // GET: Hats/Create
        //public IActionResult Create()
        //{
        //ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryID");
        // ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "SupplierID");
        // return View();
        // }
        public IActionResult Create()
        {
            PopulateCategoriesDropDownList();
            PopulateSuppliersDropDownList();
            return View();
        }

        // POST: Hats/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HatID,CategoryID,Description,HatName,Price,SupplierID")] Hat hat, IList<IFormFile> _files)
        {
            if (ModelState.IsValid)
            {
                var relativeName = "";
                var fileName = "";

                if (_files.Count < 1)
                {
                    relativeName = "/images/Default.jpg";
                }
                else
                {
                    foreach (var file in _files)
                    {
                        fileName = ContentDispositionHeaderValue
                                          .Parse(file.ContentDisposition)
                                          .FileName
                                          .Trim('"');
                        //Path for localhost
                        relativeName = "/images/HatImages/" + DateTime.Now.ToString("ddMMyyyy-HHmmssffffff") + fileName;

                        using (FileStream fs = System.IO.File.Create(_hostingEnv.WebRootPath + relativeName))
                        {
                            await file.CopyToAsync(fs);
                            fs.Flush();
                        }
                    }
                }
                hat.PathOfFile = relativeName;
                try
                {
                    if (ModelState.IsValid)
                    {
                        _context.Add(hat);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Index");
                    }
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists " + "see your system administrator.");
                }
                return View(hat);

                //_context.Add(hat);
                //await _context.SaveChangesAsync();
                //return RedirectToAction("Index");
            }
            PopulateCategoriesDropDownList(hat.CategoryID);
            PopulateSuppliersDropDownList(hat.SupplierID);
            return View(hat);
        }
        /* [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HatID,CategoryID,Description,HatName,Price,SupplierID")] Hat hat)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hat);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryID", hat.CategoryID);
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "SupplierID", hat.SupplierID);
            return View(hat);
        }*/

        // GET: Hats/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var hat = await _context.Hats
            .AsNoTracking()
            .SingleOrDefaultAsync(m => m.HatID == id);
            if (hat == null)
            {
                return NotFound();
            }
            PopulateCategoriesDropDownList(hat.CategoryID);
            PopulateSuppliersDropDownList(hat.SupplierID);
            return View(hat);
        }
        /*public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hat = await _context.Hats.SingleOrDefaultAsync(m => m.HatID == id);
            if (hat == null)
            {
                return NotFound();
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryID", hat.CategoryID);
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "SupplierID", hat.SupplierID);
            return View(hat);
        }*/

        // POST: Hats/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var hatToUpdate = await _context.Hats
            .SingleOrDefaultAsync(c => c.HatID == id);
            if (await TryUpdateModelAsync<Hat>(hatToUpdate,
            "",
            c => c.HatName, c => c.Description, c => c.Price, c => c.SupplierID, c => c.CategoryID))
            {
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.");
                }
                return RedirectToAction("Index");
            }
            PopulateCategoriesDropDownList(hatToUpdate.CategoryID);
            PopulateSuppliersDropDownList(hatToUpdate.SupplierID);
            return View(hatToUpdate);
        }
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HatID,CategoryID,Description,HatName,Price,SupplierID")] Hat hat)
        {
            if (id != hat.HatID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HatExists(hat.HatID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryID", hat.CategoryID);
            ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "SupplierID", hat.SupplierID);
            return View(hat);
        }*/

        private void PopulateCategoriesDropDownList(object selectedCategory = null)
        {
            var categoriesQuery = from d in _context.Categories
                                   orderby d.Description
                                   select d;
            ViewBag.CategoryID = new SelectList(categoriesQuery.AsNoTracking(), "CategoryID", "Description",
            selectedCategory);
        }

        private void PopulateSuppliersDropDownList(object selectedSupplier = null)
        {
            var suppliersQuery = from d in _context.Suppliers
                                  orderby d.SupplierName
                                  select d;
            ViewBag.SupplierID = new SelectList(suppliersQuery.AsNoTracking(), "SupplierID", "SupplierName",
            selectedSupplier);
        }

        // GET: Hats/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hat = await _context.Hats.SingleOrDefaultAsync(m => m.HatID == id);
            if (hat == null)
            {
                return NotFound();
            }

            return View(hat);
        }

        // POST: Hats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hat = await _context.Hats.SingleOrDefaultAsync(m => m.HatID == id);
            _context.Hats.Remove(hat);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException )
            {
                TempData["HatUsed"] = "The Hat being deleted has been used in previous orders.Delete those orders before trying again.";
                return RedirectToAction("Delete");
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool HatExists(int id)
        {
            return _context.Hats.Any(e => e.HatID == id);
        }
    }
}
