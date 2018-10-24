using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chinook_API.Models
{
    public class CustomerTotal
    {
        public decimal Total { get; set; }
        public string CustomerName{get; set;}
        public string Country{get; set;}
        public string SalesAgent{get; set;}
    }
}