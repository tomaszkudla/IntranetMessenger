using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntranetMessenger.Models
{

    public class UserRegister
    {
        [Required(ErrorMessage = "Field Name is required")]
        public string Name { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [Required(ErrorMessage ="Field Password is required")]
        public string Password { get; set; }

        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        [Required(ErrorMessage = "Field Confirm Password is required")]
        public string ConfirmPassword { get; set; }
    }
}