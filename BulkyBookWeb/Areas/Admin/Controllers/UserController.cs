using BulkyBook.Models.Models;
using BulkyBook.Models.Repository.IRepository;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using BulkyBook.Models.Repository;
using Microsoft.AspNetCore.Identity;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class UserController : Controller
    {
        private readonly ApplicationContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(ApplicationContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
           
            return View();
        }
        [HttpGet]
        public IActionResult RoleManagement(string userId)
        {
            var userObj = _db.ApplicationUsers.Where(user=>user.Id == userId).Include(u=>u.Company).FirstOrDefault();
           
            var companyList = _db.Company.Select(
                company => new SelectListItem
                {
                    Text = company.Name,
                    Value = company.Id.ToString()
                });
            var rolesList = _db.Roles.Select(
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
                var userRoleObj = _db.UserRoles.FirstOrDefault(u => u.UserId == userId);
                if(userRoleObj != null) 
                {
                    roleObj.User.Role = _db.Roles.FirstOrDefault(u => u.Id == userRoleObj.RoleId).Name;
                }
                
                return View(roleObj);
            }
            
            return View();
        }
        [HttpPost]
        public IActionResult RoleManagement(RoleManagmentVM roleVM)
        {
            var roleObj = _db.UserRoles.FirstOrDefault(u => u.UserId == roleVM.User.Id);
            if (roleObj != null)
            {
                var roleId = roleObj.RoleId;
                var oldRole = _db.Roles.FirstOrDefault(u => u.Id == roleId).Name;
                if (!(roleVM.User.Role == oldRole))
                {
                    ApplicationUser applicationUser = _db.ApplicationUsers.Where(user => user.Id == roleVM.User.Id).FirstOrDefault();
                    if (applicationUser.Role == SD.Role_Company)
                    {
                        applicationUser.CompanyId = roleVM.User.CompanyId;
                    }
                    if (oldRole == SD.Role_Company)
                    {
                        applicationUser.CompanyId = null;
                    }
                    _db.SaveChanges();
                    _userManager.RemoveFromRoleAsync(applicationUser, oldRole).GetAwaiter().GetResult();
                    _userManager.AddToRoleAsync(applicationUser, roleVM.User.Role).GetAwaiter().GetResult();
                }
            }

            return View("Index");
        }
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<ApplicationUser> objUserList = _db.ApplicationUsers.Include(u=> u.Company).ToList();
            var userRoles = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();

            foreach(var userObj in objUserList)
            {
                var roleObj = userRoles.Where(user => user.UserId == userObj.Id).FirstOrDefault();
                if(roleObj != null)
                {
                    var roleId = roleObj.RoleId;
                    userObj.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;
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
            var objFromDb = _db.ApplicationUsers.FirstOrDefault(u => u.Id == id); 
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
            _db.SaveChanges();
            return Json(new { success = true, message = "Operation Successful" });
        }
        #endregion
    }
}
