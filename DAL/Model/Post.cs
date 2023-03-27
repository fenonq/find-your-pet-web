namespace DAL.Model;

using Enum;

public class Post
{
    public int Id { get; set; }

    public DateOnly Date { get; set; }

    public string Location { get; set; }

    public string ContactNumber { get; set; }

    public PostType Type { get; set; }

    public DateOnly CreatedAt { get; set; }

    public bool IsActive { get; set; }

    public int PetId { get; set; }

    public int UserId { get; set; }

    public Pet Pet { get; set; }

    public virtual User User { get; set; }
}