using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Ziglearning.Website.Models
{
    public class EnrollModel
    {  
        
        [Required]
        public ClassModel[] Classes { get; set; }


        [Required]
        public UserModel User { get; set; }



    }
}


