using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerceAPI.Entities.DTOs
{
    public class OrderDetails
    {
        public int OrderID { get; set; }
        public string UserName { get; set; }
        public decimal TotalPrice  { get; set; }
        public string Address { get; set; }
        public  string Phone { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime DeliveredOn { get; set; }
        public bool IsDone { get; set; }
    }
}
