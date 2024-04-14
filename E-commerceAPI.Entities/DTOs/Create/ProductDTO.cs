using E_commerceAPI.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace E_commerceAPI.Entities.DTOs.Create
{
    public class ProductDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        [Display(Name = "File")]
        public IFormFile Photo { get; set; }
        public decimal Price { get; set; }
        public int CategoryID { get; set; }

    }
}
