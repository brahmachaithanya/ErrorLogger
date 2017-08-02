

namespace Models
{
    using System.ComponentModel.DataAnnotations;

    public class appmodel
    {


        [Display(Name = "User")]
        [Required(ErrorMessage = "Enter user name")]
        public string User{ get; set; }

        [Display(Name = "Application Name")]
        [Required(ErrorMessage = "Enter user password")]
        public string Application { get; set; }

        [Display(Name = "App ID")]
        [Required(ErrorMessage = "Enter user password")]
        public string App_ID { get; set; }
        
    }
}