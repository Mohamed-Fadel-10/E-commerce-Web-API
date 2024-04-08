using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerceAPI.Entities.DTOs.LogIn
{
    public class LogInDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required,EmailAddress(ErrorMessage ="Invalid Email Address")]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
