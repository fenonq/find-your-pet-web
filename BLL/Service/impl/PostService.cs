namespace BLL.Service.impl;

using DAL.Model;
using DAL.Repository;

public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;

    public PostService(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public int Add(Post post)
    {
        post.CreatedAt = DateOnly.FromDateTime(DateTime.Now);
        post.IsActive = true;
        _postRepository.Add(post);
        return post.Id;
    }

    public void ChangeVisibility(int postId)
    {
        var post = _postRepository.FindById(postId);

        if (post == null)
        {
            return;
        }

        post.IsActive = !post.IsActive;
        _postRepository.Update(post);
    }

    public List<Post> FindAll()
    {
        return _postRepository.FindAll().ToList();
    }

    public List<Post> FindAllByUserId(int userId)
    {
        return _postRepository.FindAll().Where(p => p.UserId == userId).ToList();
    }

    public List<Post> FindAllByPostType(int postType)
    {
        return _postRepository.FindAll().Where(p => (int)p.Type == postType).ToList();
    }

    public Post? FindById(int id)
    {
        return _postRepository.FindById(id);
    }

    public void Remove(int id)
    {
        _postRepository.Remove(id);
    }
}