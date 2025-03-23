using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Lathish_CRUD.Models
{
    public class Customer
    {
        public int id { get; set; }
        public string Name { get; set; }
        public int contact { get; set; }
        [Required(ErrorMessage ="Pan card is Required")]
        public int pancard { get; set; }
        public string Address { get; set; }
    }
}

 