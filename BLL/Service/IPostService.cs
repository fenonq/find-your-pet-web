using DAL.Model;

namespace BLL.Service;

public interface IPostService : ICrudService<Post>
{
    void ChangeVisibility(int postId);

    List<Post> FindAllByUserId(int userId);

    List<Post> FindAllByPostType(int postType);
}