using DAL.Model;

namespace BLL.Dto;

public class PetPostImageDto
{
    public Post? Post { get; set; }

    public Pet? Pet { get; set; }

    public Image? Image { get; set; }
}