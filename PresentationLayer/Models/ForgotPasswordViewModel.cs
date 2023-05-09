using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models;

public class ForgotPasswordViewModel
{
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; }
}
