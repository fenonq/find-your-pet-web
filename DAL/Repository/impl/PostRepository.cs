using DAL.DataContext;
using DAL.Model;

namespace DAL.Repository.impl;

public class PostRepository : IPostRepository
{
    private readonly FindYourPetContext _context;

    public PostRepository(FindYourPetContext context)
    {
        _context = context;
    }

    public IQueryable<Post> FindAll()
    {
        return _context.Posts;
    }

    public Post? FindById(int id)
    {
        return _context.Posts.Find(id);
    }

    public void Add(Post post)
    {
        _context.Posts.Add(post);
        _context.SaveChanges();
    }

    public void Update(Post post)
    {
        _context.Posts.Update(post);
        _context.SaveChanges();
    }

    public void Remove(int id)
    {
        var post = _context.Posts.Find(id);

        if (post == null)
        {
            return;
        }

        _context.Posts.Remove(post);
        _context.SaveChanges();
    }
}