using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models;

public class RegisterViewModel
{
    [Required] [Display(Name = "Name")] public string Name { get; set; }

    [Required] [Display(Name = "Surname")] public string Surname { get; set; }

    [Required] [Display(Name = "Login")] public string Login { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
        MinimumLength = 3)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }
}