using DAL.Model;
using DAL.Model.Enum;
using DAL.Repository;

namespace BLL.Service.impl;

public class PostService : IPostService
{
    private readonly IEntityRepository<Post> _postRepository;

    public PostService(IEntityRepository<Post> postRepository)
    {
        _postRepository = postRepository;
    }

    public void Add(DateTime lostDate, string location, string contactNumber,
        PostType type, DateTime createdAt, bool isActive, int petId, int userId)
    {
        var post = new Post()
        {
            LostDate = lostDate,
            Location = location,
            ContactNumber = contactNumber,
            Type = type,
            CreatedAt = createdAt,
            IsActive = isActive,
            PetId = petId,
            UserId = userId
        };

        _postRepository.Add(post);
    }

    public void ChangeVisibility(int postId)
    {
        var post = _postRepository.FindById(postId);

        if (post == null) return;
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