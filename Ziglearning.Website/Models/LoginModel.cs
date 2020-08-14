using System.ComponentModel.DataAnnotations;

namespace Ziglearning.Website.Models
{
    public class LoginModel
    {

        public int Id { get; set; }

        [Required]
        [Display(Name = "Email")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Please enter a valid email address")]
        public string Email{ get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }


    }
}