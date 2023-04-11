using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace DAL.Model;

public class User : IdentityUser<int>
{
    public string Name { get; set; }

    public string Surname { get; set; }

    [NotMapped]
    public IFormFile Photo { get; set; }

    [NotMapped]
    public string PhotoPath { get; set; }

    public virtual IEnumerable<Pet> Pets { get; } = new List<Pet>();

    public virtual IEnumerable<Post> Posts { get; } = new List<Post>();
}