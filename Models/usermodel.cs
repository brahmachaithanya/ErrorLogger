

namespace Models
{
    using System.ComponentModel.DataAnnotations;

    public class usermodel
    {   
      

        [Display(Name = "User")]
        [Required(ErrorMessage = "Enter user name")]
        public string UserName { get; set; }

        [Display(Name = "User")]
        [Required(ErrorMessage = "Enter user password")]
        public string Password { get; set; }

        [Display(Name = "Role")]
        
        public string Role { get; set; }
    }
}