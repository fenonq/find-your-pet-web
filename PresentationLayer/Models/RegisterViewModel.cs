using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models;

public class RegisterViewModel
{
    [Required]
    [Display(Name = "Name")]
    [RegularExpression(@"^[^0-9]+$", ErrorMessage = "Do not use digits")]

    public string Name { get; set; }

    [Required]
    [Display(Name = "Surname")]
    [RegularExpression(@"^[^0-9]+$", ErrorMessage = "Do not use digits")]
    public string Surname { get; set; }

    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }
}