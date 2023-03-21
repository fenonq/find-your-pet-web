using DAL.DataContext;
using DAL.Model;

namespace DAL.Repository.impl;

public class UserRepository : IEntityRepository<User>
{
    private readonly FindYourPetContext _context;

    public UserRepository(FindYourPetContext context)
    {
        _context = context;
    }
    public IQueryable<User> FindAll()
    {
        return _context.Users;
    }
    
    public User? FindById(int id)
    {
        return _context.Users.Find(id);
    }

    public void Add(User user)
    {
        Console.WriteLine("UserRepo");
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public void Update(User user)
    {
        _context.Users.Update(user);
        _context.SaveChanges();
    }

    public void Remove(int id)
    {
        var user = _context.Users.Find(id);

        if (user == null) return;
        _context.Users.Remove(user);
        _context.SaveChanges();
    }
}