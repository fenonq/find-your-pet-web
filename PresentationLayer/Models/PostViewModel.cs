using System.ComponentModel.DataAnnotations;
using DAL.Model.Enum;

namespace PresentationLayer.Models;

public class PostViewModel
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Date")]
    public DateOnly Date { get; set; }

    [Required]
    [Display(Name = "Type")]
    public PostType Type { get; set; }

    [Required]
    [Display(Name = "Location")]
    public string Location { get; set; }

    [Required]
    [RegularExpression(@"^[0-9]*$", ErrorMessage = "Do not use letters")]
    [StringLength(12, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 7)]
    [Display(Name = "Contact number")]
    public string ContactNumber { get; set; }

    [Required]
    [Display(Name = "Image")]
    public IFormFile Photo { get; set; }
}