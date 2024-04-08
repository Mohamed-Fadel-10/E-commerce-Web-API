using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerceAPI.Entities.Models
{
  
    public class Order
    {
        [Key]
        public int ID { get; set; }
        public DateTime OrderDate { get; set; }= DateTime.Now;
        public string UserName { get; set; }
        [ForeignKey("User")]
        public string UserID { get; set; }
        [MaxLength(100)]
        public string Address { get; set; }
        [MaxLength(11)]
        public string Phone { get; set; }
        public virtual IEnumerable<OrderItems> OrderItems { get; set; }
        public virtual ApplicationUser User { get; set; }




    }
}
