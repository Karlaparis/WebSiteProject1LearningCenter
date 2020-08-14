using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ziglearning.Website.Models
{
    public class ClassModel
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public ClassModel(int id, string name, string description, decimal price)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
        }

    }
}