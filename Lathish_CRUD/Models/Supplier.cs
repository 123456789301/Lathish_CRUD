using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lathish_CRUD.Models
{
    public class Supplier
    {
        public int SupplierId { get; set; }
        public string Name { get; set; }
        public string GstNo { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }
        //public List<int> ProductCategoryIds { get; set; }

        public List<int> ProductCategoryIds { get; set; } = new List<int>();
    }
}