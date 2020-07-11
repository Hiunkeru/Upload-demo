using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Upload_demo.Request
{
    public class ProductosRequest
    {
        public int Id { get; set; }
        public IFormFile Fichero { get; set; }
    }
}
