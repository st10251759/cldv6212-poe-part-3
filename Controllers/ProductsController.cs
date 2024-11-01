﻿using ABCRetailers_Cameron_Chetty_CLDV6212_POE_P3.Data;
using ABCRetailers_Cameron_Chetty_CLDV6212_POE_P3.Models;
using ABCRetailers_Cameron_Chetty_CLDV6212_POE_P3.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ABCRetailers_Cameron_Chetty_CLDV6212_POE_P3.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDBContext _context;
        private readonly BlobService _blobService;


        public ProductsController(ApplicationDBContext context, BlobService blobService)
        {
            _context = context;
            _blobService = blobService;
        }

        // GET: Products
        // GET: Products
        public IActionResult Index(string category)
        {
            IQueryable<string> categoryQuery = from p in _context.Product
                                               orderby p.Category
                                               select p.Category;

            var distinctCategories = categoryQuery.Distinct().ToList();

            ViewBag.Category = new SelectList(distinctCategories);

            IQueryable<Product> products = _context.Product;

            if (!string.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.Category == category);
            }

            return View(products.ToList());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,Name,ProductDescription,Price,Category,Availability,ImageUrlpath")] Product product, IFormFile file)
        {
            // Handle file upload if file is provided
            if (file != null && file.Length > 0)
            {
                // Check if file is an image
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var extension = Path.GetExtension(file.FileName).ToLower();

                if (allowedExtensions.Contains(extension))
                {
                    using var stream = file.OpenReadStream();
                    var imageUrl = await _blobService.UploadAsync(stream, file.FileName);
                    product.ImageUrlpath = imageUrl;
                }
                else
                {
                    ModelState.AddModelError("file", "Only image files (.jpg, .jpeg, .png, .gif) are allowed.");
                    return View(product);
                }
            }

            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,ProductDescription,Price,Category,Availability")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            // Fetch the existing product to retain the ImageUrlpath
            var existingProduct = await _context.Product.AsNoTracking().FirstOrDefaultAsync(p => p.ProductId == id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            // Preserve the ImageUrlpath
            product.ImageUrlpath = existingProduct.ImageUrlpath;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                // Delete the image in Blob Storage if it exists
                if (!string.IsNullOrEmpty(product.ImageUrlpath))
                {
                    await _blobService.DeleteBlobAsync(product.ImageUrlpath);
                }

                _context.Product.Remove(product);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ProductId == id);
        }
    }
}