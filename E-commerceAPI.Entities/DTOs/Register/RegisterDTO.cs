using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerceAPI.Entities.DTOs.Register
{
    public class RegisterDTO
    {
        [Required,MaxLength(15)]
        public string FirstName { get; set; }
        [Required, MaxLength(15)]
        public string LastName { get; set; }
        [Required, MaxLength(11)]
        public string Phone { get; set; }
        [Required,EmailAddress]
        public string Email { get; set; }
        public string UserName  { get; set; }
        [Required, MaxLength(15)]
        public string Password { get; set; }
        [Required, MaxLength(15),Compare("Password")]
        public string ConfirmPassword { get; set; }
        public string[] Roles { get; set; }


    }
}
