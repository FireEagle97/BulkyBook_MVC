using BulkyBook.Models.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Security.Cryptography.Xml;
//using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BulkyBook.Models.ViewModels
{
    public class ProductVM
    {
        public Product Product { get; set; } = new Product();
        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        //[ValidateNever]
        //public IEnumerable<SelectListItem> CoverTypeList { get; set; }


    }
}
