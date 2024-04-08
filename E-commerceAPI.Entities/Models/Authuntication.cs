﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerceAPI.Entities.Models
{
    public class Authuntication
    {
        public string UserName { get; set; }

        public string Email { get; set; }
        public string Message { get; set; }

        public bool IsAuthenticated { get; set; }
        public List<string> Roles { get; set; }


    }
}
