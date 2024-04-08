using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerceAPI.Entities.Models
{
    public class Products_Carts
    {
        [Key]
        public int ID { get; set; }
        public virtual Product Product { get; set; }
        public virtual  Cart Cart { get; set; }
        [ForeignKey("Product")]
        public int ProductID { get; set; }
        [ForeignKey("Cart")]
        public int CartID { get; set; }

    }
}
