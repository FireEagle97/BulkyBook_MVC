//using BulkyBook.Models.Models;
//using BulkyBook.Models.Repository.IRepository;
//using BulkyBook.Models.ViewModels;
//using BulkyBook.Utility;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.Extensions.Hosting;
//using System.Text.Json.Serialization;
//using System.Text.Json;

//namespace BulkyBookWeb.Areas.Admin.Controllers
//{
//    [Area("Admin")]
//    //[Authorize(Roles = SD.Role_Admin)]
//    public class CompanyController : Controller
//    {
//        private readonly IUnitOfWork _unitOfWork;

//        public CompanyController(IUnitOfWork unitOfWork)
//        {
//            _unitOfWork = unitOfWork;
//        }
//        public IActionResult Index()
//        {
//            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
//            return View(objCompanyList);
//        }
//        public IActionResult Upsert(int? id)
//        {
//            if (id == null || id == 0)
//            {
//                //create
//                return View(new Company());
//            }
//            else
//            {
//                //update
//                var companyFromDb = _unitOfWork.Company.Get(x => x.Id == id);
//                return View(companyFromDb);
//            }

//        }
//        [HttpPost]
//        public IActionResult Upsert(Company obj)
//        {
//            if (ModelState.IsValid)
//            {
//                if (obj.Id == 0)
//                {
//                    _unitOfWork.Company.Add(obj);
//                }
//                else
//                {
//                    _unitOfWork.Company.Update(obj);
//                }
//                _unitOfWork.Save();
//                TempData["success"] = "Company created successfully";
//                return RedirectToAction("Index");
//            }
//            return View();
//        }
//        #region API CALLS
//        [HttpGet]
//        public IActionResult GetAll()
//        {
//            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
//            JsonSerializerOptions options = new()
//            {
//                ReferenceHandler = ReferenceHandler.IgnoreCycles,
//                WriteIndented = true
//            };
//            return Json(new { data = objCompanyList }, options);
//        }
//        [HttpDelete]
//        public IActionResult Delete(int id)
//        {
//            var companyToBeDeleted = _unitOfWork.Company.Get(u => u.Id == id);
//            if (companyToBeDeleted == null)
//            {
//                return Json(new { success = false, message = "Error while deleting" });
//            }

//            _unitOfWork.Company.Remove(companyToBeDeleted);
//            _unitOfWork.Save();
//            return Json(new { success = true, message = "Delete Successful" });
//        }
//        #endregion
//    }
//}
