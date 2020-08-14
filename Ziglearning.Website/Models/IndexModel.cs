using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ziglearning.Website.Models
{
    public class IndexModel
    {

        public ClassModel[] Classes { get; set; }

        public UserModel[] Users { get; set; }
    }
}