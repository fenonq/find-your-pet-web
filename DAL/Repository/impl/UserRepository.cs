namespace DAL.Repository.impl;

using DAL.DataContext;
using DAL.Model;

public class UserRepository : IEntityRepository<User>
{
    private readonly FindYourPetContext _context;

    public UserRepository(FindYourPetContext context)
    {
        this._context = context;
    }

    public IQueryable<User> FindAll()
    {
        return this._context.Users;
    }

    public User? FindById(int id)
    {
        return this._context.Users.Find(id);
    }

    public void Add(User user)
    {
        Console.WriteLine("UserRepo");
        this._context.Users.Add(user);
        this._context.SaveChanges();
    }

    public void Update(User user)
    {
        this._context.Users.Update(user);
        this._context.SaveChanges();
    }

    public void Remove(int id)
    {
        var user = this._context.Users.Find(id);

        if (user == null)
        {
            return;
        }

        this._context.Users.Remove(user);
        this._context.SaveChanges();
    }
}