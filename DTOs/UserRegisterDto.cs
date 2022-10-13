using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.DTOs
{
    public class UserRegisterDto
    {
        [Required]
        public string username { get; set; }
        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "Password must be between 4 and 8")]
        public string password { get; set; }
    }
}