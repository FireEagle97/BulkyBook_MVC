using BulkyBook.Models.Models;
//using BulkyBook.Models.Repository.IRepository;
//using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    public class CategoryController : Controller
    {
        private ApplicationContext _db;

        public CategoryController(ApplicationContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _db.Category.ToList();
            //List<CategoryViewModel> objCategoryViewModels = new List<CategoryViewModel>();
            //foreach (var objCategory in objCategoryList)
            //{
            //    var category = new CategoryViewModel
            //    {
            //        Id = objCategory.Id,
            //        Name = objCategory.Name,
            //        DisplayOrder = objCategory.DisplayOrder
            //    };
            //    objCategoryViewModels.Add(category);

            //}

            return View(objCategoryList);
        }
        //GET
        public IActionResult Create()
        {
            return View();
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The Display Order cannot exactly match the Name.");
            }
            if (ModelState.IsValid)
            {
                var InputUser = new Category
                {
                    Name = obj.Name,
                    DisplayOrder = obj.DisplayOrder,
                    CreatedAt = DateTime.Now
                };
                _db.Category.Add(InputUser);
                _db.SaveChanges();
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");
            }
            return View(obj);

        }
        //GET
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var categoryFromDb = _db.Category.FirstOrDefault(u => u.Id == id);
            if (categoryFromDb == null)
            {
                return NotFound();

            }
            var categoryViewModel = new Category
            {
                Id = categoryFromDb.Id,
                Name = categoryFromDb.Name,
                DisplayOrder = categoryFromDb.DisplayOrder,
            };

            return View(categoryViewModel);
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The Display Order cannot exactly match the Name.");
            }
            if (ModelState.IsValid)
            {
                var InputToBeEdited = _db.Category.FirstOrDefault(u => u.Id == obj.Id);
                if (InputToBeEdited != null)
                {
                    InputToBeEdited.Name = obj.Name;
                    InputToBeEdited.DisplayOrder = obj.DisplayOrder;
                    _db.Category.Update(InputToBeEdited);
                    _db.SaveChanges();
                }
                TempData["success"] = "Category edited successfully";
                return RedirectToAction("Index");
            }
            return View(obj);

        }
        //GET
        //GET
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var categoryFromDb = _db.Category.FirstOrDefault(u => u.Id == id);
            if (categoryFromDb == null)
            {
                return NotFound();

            }
            var categoryViewModel = new Category
            {
                Id = categoryFromDb.Id,
                Name = categoryFromDb.Name,
                DisplayOrder = categoryFromDb.DisplayOrder,
            };

            return View(categoryViewModel);
        }
        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.Category.FirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.Category.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
