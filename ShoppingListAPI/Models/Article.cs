using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingListAPI.Models
{
    public class Article
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Amount { get; set; }
        public string Remark { get; set; } 
        public bool IsBought { get; set; }
    }
}
