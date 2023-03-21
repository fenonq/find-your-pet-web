namespace DAL.Model;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }

    public virtual IEnumerable<Pet> Pets { get; } = new List<Pet>();
    public virtual IEnumerable<Post> Posts { get; } = new List<Post>();
}