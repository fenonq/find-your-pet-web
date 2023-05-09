using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models;

public class PetViewModel
{
    [Required]
    [RegularExpression(@"^[^0-9]+$", ErrorMessage = "Do not use digits")]
    [Display(Name = "Name")]
    public string Name { get; set; }

    [Required]
    [RegularExpression(@"^[0-9]*$", ErrorMessage = "Do not use letters")]
    [Display(Name = "Age")]
    public int Age { get; set; }

    [Required]
    [Display(Name = "Description")]
    public string Description { get; set; }
}