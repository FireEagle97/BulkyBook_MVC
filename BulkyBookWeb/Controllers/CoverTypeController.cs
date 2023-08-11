using BulkyBook.Models.Models;
//using BulkyBookWeb.Models.Repository.IRepository;
//using BulkyBookWeb.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    public class CoverTypeController : Controller
    {
        private ApplicationContext _db;

        public CoverTypeController(ApplicationContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<CoverType> objCoverTypeList = _db.CoverType.ToList();
            //List<CoverTypeViewModel> objCoverTypeViewModels = new List<CoverTypeViewModel>();
            //foreach (var objCoverType in objCoverTypeList)
            //{
            //    var coverType = new CoverTypeViewModel
            //    {
            //        Id = objCoverType.Id,
            //        Name = objCoverType.Name,
            //    };
            //    objCoverTypeViewModels.Add(coverType);

            //}

            return View(objCoverTypeList);
        }
        //GET
        public IActionResult Create()
        {
            return View();
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CoverType obj)
        {
            if (ModelState.IsValid)
            {
                var InputUser = new CoverType
                {
                    Name = obj.Name,
                };
                _db.CoverType.Add(InputUser);
                _db.SaveChanges();
                TempData["success"] = "Cover Type created successfully";
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
            var coverTypeFromDb = _db.CoverType.FirstOrDefault(x => x.Id == id);
            if (coverTypeFromDb == null)
            {
                return NotFound();

            }
            //var coverTypeViewModel = new CoverTypeViewModel
            //{
            //    Id = coverTypeFromDb.Id,
            //    Name = coverTypeFromDb.Name,
            //};

            return View(coverTypeFromDb);
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CoverType obj)
        {
            if (ModelState.IsValid)
            {
                var InputToBeEdited = _db.CoverType.FirstOrDefault(u => u.Id == obj.Id);
                if (InputToBeEdited != null)
                {
                    InputToBeEdited.Name = obj.Name;
                    _db.CoverType.Update(InputToBeEdited);
                    _db.SaveChanges();
                }
                TempData["success"] = "Cover Type edited successfully";
                return RedirectToAction("Index");
            }
            return View(obj);

        }

        //GET
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var coverTypeFromDb = _db.CoverType.FirstOrDefault(u => u.Id == id);
            if (coverTypeFromDb == null)
            {
                return NotFound();

            }
            var coverTypeViewModel = new CoverType
            {
                Id = coverTypeFromDb.Id,
                Name = coverTypeFromDb.Name,

            };

            return View(coverTypeViewModel);
        }
        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.CoverType.FirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.CoverType.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Cover Type deleted successfully";
            return RedirectToAction("Index");
        }
    }
}