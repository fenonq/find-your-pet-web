namespace DAL.Repository.impl;

using DataContext;
using Model;

public class PostRepository : IEntityRepository<Post>
{
    private readonly FindYourPetContext _context;

    public PostRepository(FindYourPetContext context)
    {
        this._context = context;
    }

    public IQueryable<Post> FindAll()
    {
        return this._context.Posts;
    }

    public Post? FindById(int id)
    {
        return this._context.Posts.Find(id);
    }

    public void Add(Post post)
    {
        this._context.Posts.Add(post);
        this._context.SaveChanges();
    }

    public void Update(Post post)
    {
        this._context.Posts.Update(post);
        this._context.SaveChanges();
    }

    public void Remove(int id)
    {
        var post = this._context.Posts.Find(id);

        if (post == null)
        {
            return;
        }

        this._context.Posts.Remove(post);
        this._context.SaveChanges();
    }
}