<<<<<<<< HEAD:BulkyBook.Models/Models/Company.cs
﻿using System;
========
﻿using BulkyBook.Models.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
>>>>>>>> 40a13f6fd53ac9f369cd713ba1babac467684b6e:BulkyBook.Models/ApplicationUser.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
<<<<<<<< HEAD:BulkyBook.Models/Models/Company.cs
        public string? PostalCode { get; set; }
        public string? PhoneNumber { get; set; }
========
        public string? PostalCode { get; set;}
        public int? CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        [ValidateNever]
        public Company? Company { get; set; }

>>>>>>>> 40a13f6fd53ac9f369cd713ba1babac467684b6e:BulkyBook.Models/ApplicationUser.cs
        
    }
}