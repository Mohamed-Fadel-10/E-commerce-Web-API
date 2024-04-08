using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerceAPI.Entities.Models
{
    public class Product
    {
        [Key]
        public int ID { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        public string Description { get; set; }
        public byte [] Photo { get; set; }
        public decimal Price { get; set; }
        [ForeignKey("Category")]
        public int CategoryID { get; set; }
        public virtual Category Category  { get; set; }
        public virtual IEnumerable<OrderItems> OrderItems { get; set; }
        public virtual IEnumerable<Products_Carts> Products_Carts { get; set; }


    }
}
