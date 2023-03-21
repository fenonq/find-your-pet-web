using DAL.Model;
using DAL.Model.Enum;

namespace BLL.Service;

public interface IPostService
{
    void Add(DateTime lostDate, string location, string contactNumber,
        PostType type, DateTime createdAt, bool isActive, int petId, int userId);

    void ChangeVisibility(int postId);

    List<Post> FindAll();

    List<Post> FindAllByUserId(int userId);

    List<Post> FindAllByPostType(int postType);

    Post? FindById(int id);

    void Remove(int id);
}