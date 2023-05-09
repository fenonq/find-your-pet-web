using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace PresentationLayer.Models;

public class ResetPassword
{
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Required]
    //[StringLength(100, ErrorMessage = "The {0} must be at least {4} characters long.", MinimumLength = 4)]
    [Display(Name = "Password")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "Password and Confirm Password must match")]
    public string ConfirmPassword { get; set; }

    public string Token { get; set; }
}
