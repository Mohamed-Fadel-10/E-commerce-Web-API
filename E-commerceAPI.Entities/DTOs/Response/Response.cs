using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerceAPI.Entities.DTOs.Response
{
    public  class Response
    {
        public string  Message { get; set; }
        public int StatusCode { get; set; }
        public object? Model { get; set; }
        public bool isDone { get; set; } = false;
        public List<string>? Errors { get; set; }
        public List<string>? Items { get; set;}
    }
}
