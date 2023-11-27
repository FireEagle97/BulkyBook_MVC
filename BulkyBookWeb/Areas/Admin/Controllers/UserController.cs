using BulkyBook.Models.Models;
using BulkyBook.Models.Repository.IRepository;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json.Serialization;
using System.Text.Json;
using BulkyBook.Models.Repository;
using Microsoft.AspNetCore.Identity;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //_db = db;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
           
            return View();
        }
        [HttpGet]
        public IActionResult RoleManagement(string userId)
        {
            var userObj = _unitOfWork.ApplicationUser.Get(user => user.Id == userId, includeProperties: "Company");
           
            var companyList = _unitOfWork.Company.GetAll().Select(
                company => new SelectListItem
                {
                    Text = company.Name,
                    Value = company.Id.ToString()
                });
            var rolesList = _roleManager.Roles.Select(
                    role => new SelectListItem
                    {
                        Text = role.Name,
                        Value = role.Name
                    }
                );
           
            if(userObj != null)
            {
                RoleManagmentVM roleObj = new RoleManagmentVM
                {
                    User = userObj,
                    CompanyList = companyList,
                    RoleList = rolesList
                };
                roleObj.User.Role = _userManager.GetRolesAsync(_unitOfWork.ApplicationUser.Get(u => u.Id == userId)).GetAwaiter().GetResult().FirstOrDefault();

                return View(roleObj);
            }
            
            return View();
        }
        [HttpPost]
        public IActionResult RoleManagement(RoleManagmentVM roleVM)
        {
            string oldRole = _userManager.GetRolesAsync(_unitOfWork.ApplicationUser.Get(u => u.Id == roleVM.User.Id)).GetAwaiter().GetResult().FirstOrDefault();
            ApplicationUser applicationUser = _unitOfWork.ApplicationUser.Get(user => user.Id == roleVM.User.Id);
            if (!(roleVM.User.Role == oldRole))
            {

                if (applicationUser.Role == SD.Role_Company)
                {
                    applicationUser.CompanyId = roleVM.User.CompanyId;
                }
                if (oldRole == SD.Role_Company)
                {
                    applicationUser.CompanyId = null;
                }
                _unitOfWork.ApplicationUser.Update(applicationUser);
                _unitOfWork.Save();
                _userManager.RemoveFromRoleAsync(applicationUser, oldRole).GetAwaiter().GetResult();
                _userManager.AddToRoleAsync(applicationUser, roleVM.User.Role).GetAwaiter().GetResult();
            }else if (oldRole ==SD.Role_Company && applicationUser.CompanyId != roleVM.User.CompanyId)
            {
                applicationUser.CompanyId=roleVM.User.CompanyId;
                _unitOfWork.ApplicationUser.Update(applicationUser);
                _unitOfWork.Save();
            }

            return View("Index");
        }
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<ApplicationUser> objUserList = _unitOfWork.ApplicationUser.GetAll(includeProperties: "Company").ToList();
            foreach(var userObj in objUserList)
            {

                userObj.Role = _userManager.GetRolesAsync(userObj).GetAwaiter().GetResult().FirstOrDefault();
                if (userObj.Company == null)
                {
                    userObj.Company = new Company()
                    {
                        Name = ""
                    };
                }
            }
   
            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };
            return Json(new { data = objUserList }, options);
        }
        [HttpPost]
        public IActionResult LockUnlock([FromBody]string id)
        {
            var objFromDb = _unitOfWork.ApplicationUser.Get(u => u.Id == id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while Locking/Unlocking" });
            }
            if(objFromDb.LockoutEnd != null && objFromDb.LockoutEnd > DateTime.Now)
            {
                objFromDb.LockoutEnd = DateTime.Now;
            }
            else
            {
                objFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
            }
            _unitOfWork.ApplicationUser.Update(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Operation Successful" });
        }
        #endregion
    }
}
