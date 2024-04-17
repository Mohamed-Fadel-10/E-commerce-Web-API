using E_commerceAPI.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace E_commerceAPI.Entities.DTOs.Create
{
    public class OrderDTO
    {
        [JsonIgnore]
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public string FullName { get; set; }
        [MaxLength(100)]
        public string Address { get; set; }
        [MaxLength(11)]
        public string Phone { get; set; }

    }
}
