using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProductManage.Models;
using ProductManage.Models.ViewModels;

namespace ProductManage.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment; // 新增：用於獲取 wwwroot 路徑
        public ProductsController(ProductDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment=webHostEnvironment;
        }

        public async Task<IActionResult> Index(int? selectedCategoryId, string? searchName, string? sortOrder, int pageNumber = 1)
        {
            // --- 新增：定義每頁筆數 ---
            const int pageSize = 6;

            // 步驟1：準備 IQueryable 查詢
            IQueryable<Product> productsQuery = _context.Products.Include(p => p.Category);

            // 步驟2：根據篩選和搜尋條件應用 Where 語句
            if (selectedCategoryId.HasValue && selectedCategoryId.Value > 0)
            {
                productsQuery = productsQuery.Where(p => p.CategoryId == selectedCategoryId.Value);
            }
            if (!string.IsNullOrEmpty(searchName))
            {
                productsQuery = productsQuery.Where(p => p.Name.Contains(searchName));
            }

            // --- 新增：在排序前先取得總筆數，以計算總頁數 ---
            var totalItems = await productsQuery.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            // 步驟3：根據 sortOrder 參數進行排序
            switch (sortOrder)
            {
                case "price_desc":
                    productsQuery = productsQuery.OrderByDescending(p => p.Price);
                    break;
                case "price_asc":
                default:
                    productsQuery = productsQuery.OrderBy(p => p.Price);
                    break;
            }

            // --- 新增：應用分頁邏輯（Skip 和 Take） ---
            var products = await productsQuery.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            // 步驟4：將結果轉換為 ViewModel
            var productViewModel = products.Select(p => new ProductViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                CategoryName = p.Category.Name,
                ImageUrl = p.ImageUrl
            }).ToList();

            // 步驟5：準備下拉式選單和 ViewModel
            var categories = await _context.Categories
                                           .Select(c => new SelectListItem
                                           {
                                               Value = c.Id.ToString(),
                                               Text = c.Name
                                           })
                                           .ToListAsync();

            categories.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "-- 請選擇分類 --"
            });

            var model = new ProductListViewModel
            {
                Products = productViewModel,
                Categories = categories,
                SelectedCategoryId = selectedCategoryId,
                SearchName = searchName,
                PriceSortParam = sortOrder == "price_asc" ? "price_desc" : "price_asc",

                // --- 新增：將分頁資訊傳入 ViewModel ---
                PageNumber = pageNumber,
                TotalPages = totalPages,
                PageSize = pageSize,
                // --- 新增：將總筆數傳入 ViewModel ---
                TotalItems = totalItems
            };

            return View(model);
        }


        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public async Task<IActionResult> Create()
        {
            var findCategories = await _context.Categories.ToListAsync();
            var productViewModel = new ProductViewModel
            {
                Categories = findCategories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList()
            };
            
            return View(productViewModel);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel viewproduct)
        {
            if (ModelState.IsValid)
            {
                var products = new Product
                {
                    Name = viewproduct.Name,
                    Price = viewproduct.Price ?? 0,
                    Description = viewproduct.Description,
                    CreatedAt = DateTime.Now,
                    CategoryId = viewproduct.CategoryId.GetValueOrDefault()
                };

                // --- 新增：處理檔案上傳邏輯 ---
                if (viewproduct.ImageFile != null)
                {
                    // 1. 定義檔案儲存路徑
                    string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }

                    // 2. 建立獨特的檔案名稱，以避免重複
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + viewproduct.ImageFile.FileName;
                    string filePath = Path.Combine(uploadFolder, uniqueFileName);

                    // 3. 將檔案儲存到指定路徑
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await viewproduct.ImageFile.CopyToAsync(fileStream);
                    }

                    // 4. 將圖片路徑儲存到 Model
                    products.ImageUrl = "/images/" + uniqueFileName;
                }
                // --- 檔案上傳結束 ---

                _context.Add(products);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // 如果驗證失敗，重新載入分類清單
            viewproduct.Categories = await _context.Categories
                                        .Select(c => new SelectListItem
                                        {
                                            Value = c.Id.ToString(),
                                            Text = c.Name
                                        }).ToListAsync();

            return View(viewproduct);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            // 取得所有分類
            var categories = await _context.Categories
                                         .Select(c => new SelectListItem
                                         {
                                             Value = c.Id.ToString(),
                                             Text = c.Name
                                         }).ToListAsync();

            var productviewmodel = new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                CategoryId = product.CategoryId,
                ImageUrl = product.ImageUrl, // 將現有圖片路徑傳入 ViewModel
                Categories = categories
            };

            return View(productviewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductViewModel product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingProduct = await _context.Products.FindAsync(id);
                    if (existingProduct == null)
                    {
                        return NotFound();
                    }

                    existingProduct.Name = product.Name;
                    existingProduct.Price = product.Price ?? 0;
                    existingProduct.Description = product.Description;
                    existingProduct.CategoryId = product.CategoryId.GetValueOrDefault();

                    // --- 新增：處理檔案上傳邏輯 ---
                    if (product.ImageFile != null)
                    {
                        // 如果有舊圖片，先刪除
                        if (!string.IsNullOrEmpty(existingProduct.ImageUrl))
                        {
                            string oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, existingProduct.ImageUrl.TrimStart('/'));
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }

                        // 儲存新圖片
                        string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                        if (!Directory.Exists(uploadFolder))
                        {
                            Directory.CreateDirectory(uploadFolder);
                        }
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + product.ImageFile.FileName;
                        string filePath = Path.Combine(uploadFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await product.ImageFile.CopyToAsync(fileStream);
                        }

                        // 更新圖片路徑
                        existingProduct.ImageUrl = "/images/" + uniqueFileName;
                    }
                    // --- 檔案上傳結束 ---

                    _context.Products.Update(existingProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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

            // 如果驗證失敗，重新載入分類清單
            product.Categories = await _context.Categories
                                        .Select(c => new SelectListItem
                                        {
                                            Value = c.Id.ToString(),
                                            Text = c.Name
                                        }).ToListAsync();
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
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
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
