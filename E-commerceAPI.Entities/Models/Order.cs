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
        public Order()
        {
            this.IsDone= DateTime.Now == this.CreatedOn.AddDays(8)? true : false;
        }

        [Key]
        public int ID { get; set; }
        public DateTime CreatedOn { get; set; }= DateTime.Now;
        public DateTime DeliveredOn { get; set; } = DateTime.Now.AddDays(7);
        public bool IsDone { get; set; }
        public string UserName { get; set; }
        [ForeignKey("User")]
        public string UserID { get; set; }
        [MaxLength(100)]
        public string Address { get; set; }
        [MaxLength(11)]
        public string Phone { get; set; }
        public decimal TotalPrice { get; set; }
        public virtual IEnumerable<OrderItems> OrderItems { get; set; }
        public virtual ApplicationUser User { get; set; }


            

    }
}
