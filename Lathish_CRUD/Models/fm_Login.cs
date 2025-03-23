using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Lathish_CRUD.Models
{
    public class fm_Login
    {
        public int UserID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Phone]
        public string ContactNo { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

         public string PasswordHash { get; set; }
    }
}

 

 
  
 