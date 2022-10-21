using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ProjectMVC5.Models
{
    public class UserLogin
    {

        [Required(ErrorMessage = "The Email field is required")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}