using BulkyBook.Models;
using BulkyBook.Models.Repository.IRepository;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using BulkyBook.Models.Models;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        //private readonly ApplicationContext _db;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();

            return View(objProductList);
        }
        public IActionResult Upsert(int? id)
        {
            var categoryList = _unitOfWork.Category.GetAll().Select(
            category => new SelectListItem
            {
                Text = category.Name,
                Value = category.Id.ToString()

            });
            ProductVM prodVM = new ProductVM
            {
                CategoryList = categoryList
            };
            if (id == null || id == 0)
            {
                //create
                return View(prodVM);
            }
            else
            {
                //update
                prodVM.Product = _unitOfWork.Product.Get(x => x.Id == id);
                return View(prodVM);
            }

        }
        [HttpPost]
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");
                    if (!string.IsNullOrEmpty(obj.Product.ImageUrl))
                    {
                        //delete the old image
                        var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var filestream = new FileStream(Path.Combine(productPath, filename), FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }
                    obj.Product.ImageUrl = @"\images\product\" + filename;
                }
                if(obj.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(obj.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(obj.Product);
                }
                //var prodObj = new Product
                //{
                //    Title = obj.Product.Title,
                //    Description = obj.Product.Description,
                //    Isbn = obj.Product.Isbn,
                //    Author = obj.Product.Author,
                //    Price = obj.Product.Price,
                //    ListPrice = obj.Product.ListPrice,
                //    Price100 = obj.Product.Price100,
                //    Price50 = obj.Product.Price50,
                //    CategoryId = obj.Product.CategoryId,
                //    ImageUrl = obj.Product.ImageUrl,
                //    //CoverTypeId = obj.CoverTypeId
                //};
                //_db.Product.Add(prodObj);
                //_db.SaveChanges();
               
                _unitOfWork.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            Product? productFromDb = _unitOfWork.Product.Get(u => u.Id == id);
            if (productFromDb == null)
            {
                return NotFound();
            }
            var categoryList = _unitOfWork.Category.GetAll().Select(
            category => new SelectListItem
            {
                Text = category.Name,
                Value = category.Id.ToString()

            });
            ProductVM prodVM = new ProductVM
            {
                Product = productFromDb,
                CategoryList = categoryList
            };
            //ProductViewModel prodVmObj = new ProductViewModel
            //{
            //    Title = productFromDb.Title,
            //    Description = productFromDb.Description,
            //    Author = productFromDb.Author,
            //    Price = productFromDb.Price,
            //    ListPrice = productFromDb.ListPrice,
            //    Price100 = productFromDb.Price100,
            //    Price50 = productFromDb.Price50,
            //    CategoryId = productFromDb.CategoryId,
            //    CoverTypeId = productFromDb.CoverTypeId
            //};
            return View(prodVM);
        }
        [HttpPost]
        public IActionResult Edit(ProductVM obj)
        {
            if(ModelState.IsValid)
            {
                var InputToBeEdited = _unitOfWork.Product.Get(u => u.Id == obj.Product.Id);
                if(InputToBeEdited != null)
                {
                    InputToBeEdited.Title = obj.Product.Title;
                    InputToBeEdited.Description = obj.Product.Description; 
                    InputToBeEdited.Author = obj.Product.Author;
                    InputToBeEdited.Price = obj.Product.Price;
                    InputToBeEdited.ListPrice = obj.Product.ListPrice;
                    InputToBeEdited.Price50 = obj.Product.Price50;
                    InputToBeEdited.Price100 = obj.Product.Price100;
                    InputToBeEdited.CategoryId = obj.Product.CategoryId;
                    //InputToBeEdited.CoverTypeId = obj.Product.CoverTypeId;

                }
                _unitOfWork.Product.Update(obj.Product);
                _unitOfWork.Save();
                TempData["success"] = "Product updated successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };
            return Json(new { data = objProductList }, options);
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var productToBeDeleted = _unitOfWork.Product.Get(u => u.Id == id);
            if(productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            if(productToBeDeleted.ImageUrl != null)
            {
                var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, productToBeDeleted.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }
    
            _unitOfWork.Product.Remove(productToBeDeleted);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }
}