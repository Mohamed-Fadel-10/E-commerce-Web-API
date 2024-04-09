using E_commerceAPI.Entities.Models.JWT_Token;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerceAPI.Entities.Models
{
    public class ApplicationUser:IdentityUser
    {
        [Required, MaxLength(15)]

        public string FirstName { get; set; }
        [Required, MaxLength(15)]
        public string LastName { get; set; }
        public virtual IEnumerable<Order> Orders { get; set; }
        public List<RefreshToken>? RefreshTokens  { get; set; }

    }
}
