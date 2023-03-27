namespace BLL.Service;

using DAL.Model;

public interface IPostService : ICrudService<Post>
{
    void ChangeVisibility(int postId);

    List<Post> FindAllByUserId(int userId);

    List<Post> FindAllByPostType(int postType);
}