using E_commerceAPI.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace E_commerceAPI.Entities.DTOs.Create
{
    public class CategoryDTO
    {
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }

    }
}
