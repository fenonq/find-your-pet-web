using System.ComponentModel.DataAnnotations;
using DAL.Model.Enum;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace PresentationLayer.Models;

public class PostViewModel
{
    [Display(Name = "Id")]
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
    [Display(Name = "ContactNumber")]
    public string ContactNumber { get; set; }

    [Display(Name = "Image")]
    public IFormFile Photo { get; set; }
}