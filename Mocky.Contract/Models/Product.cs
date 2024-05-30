using System.Collections.Generic;

namespace Mocky.Contract.Models
{
    public class Product
    {
        public string title { get; set; }
        public decimal price { get; set; }
        public List<string> sizes { get; set; }
        public string description { get; set; }
    }
}
