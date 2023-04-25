using System.ComponentModel.DataAnnotations;
using Microsoft.Build.Framework;

namespace PresentationLayer.Models;

public class ForgotPasswordViewModel
{
    [System.ComponentModel.DataAnnotations.Required]
    [EmailAddress]

    public string Email { get; set; }
}
