using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IntranetMessenger.Models
{
    public class UserLog
    {
        
        [Required(ErrorMessage = "Field Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Field Password is required")]
        public string Password { get; set; }
    }
}