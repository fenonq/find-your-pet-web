namespace DAL.Model;

public class Pet
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int Age { get; set; }

    public string Description { get; set; }

    public int OwnerId { get; set; }

    public virtual User User { get; set; }

    public virtual Image Image { get; set; }

    public virtual ICollection<Post> Posts { get; } = new List<Post>();
}