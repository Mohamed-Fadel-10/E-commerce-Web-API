using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerceAPI.Entities.DTOs
{
    public class WelcomeRequestDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }

    }
}
