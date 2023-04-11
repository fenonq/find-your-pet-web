using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models;

public class PetViewModel
{
    [Display(Name = "Name")] public string Name { get; set; }

    [Display(Name = "Age")] public int Age { get; set; }

    [Required]
    [Display(Name = "Description")]
    public string Description { get; set; }
}