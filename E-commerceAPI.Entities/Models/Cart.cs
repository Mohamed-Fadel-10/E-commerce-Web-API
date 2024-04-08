using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerceAPI.Entities.Models
{
    public class Cart
    {
        [Key]
        public int ID { get; set; }
        public virtual ApplicationUser User { get; set; }
        [ForeignKey("User")]
        public string UserID { get; set; }
        public virtual IEnumerable<Products_Carts> Products_Carts { get; set; }

    }
}
