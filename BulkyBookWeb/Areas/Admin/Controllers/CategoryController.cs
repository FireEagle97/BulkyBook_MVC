using BulkyBook.Models.Models;
using BulkyBook.Models.Repository.IRepository;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
//using BulkyBook.Models.Repository.IRepository;
//using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
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
                _unitOfWork.Category.Add(InputUser);
                _unitOfWork.Category.Save();
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
            var categoryFromDb = _unitOfWork.Category.Get(u => u.Id == id);
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
                var InputToBeEdited = _unitOfWork.Category.Get(u => u.Id == obj.Id);
                if (InputToBeEdited != null)
                {
                    InputToBeEdited.Name = obj.Name;
                    InputToBeEdited.DisplayOrder = obj.DisplayOrder;
                    _unitOfWork.Category.Update(InputToBeEdited);
                    _unitOfWork.Category.Save();
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
            var categoryFromDb = _unitOfWork.Category.Get(u => u.Id == id);
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
            var obj = _unitOfWork.Category.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Category.Save();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
