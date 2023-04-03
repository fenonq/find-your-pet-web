namespace DAL.Model;

public class Image
{
    public int Id { get; set; }

    public string Path { get; set; }

    public int PetId { get; set; }

    public virtual Pet Pet { get; set; }
    
    public override string ToString()
    {
        return $"Image(Id={Id}, Path={Path}, PetId={PetId})";
    }
}