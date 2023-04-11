using DAL.DataContext;
using DAL.Model;

namespace DAL.Repository.impl;

public class PetRepository : IPetRepository
{
    private readonly FindYourPetContext _context;

    public PetRepository(FindYourPetContext context)
    {
        _context = context;
    }

    public IQueryable<Pet> FindAll()
    {
        return _context.Pets;
    }

    public Pet? FindById(int id)
    {
        return _context.Pets.Find(id);
    }

    public void Add(Pet pet)
    {
        _context.Pets.Add(pet);
        _context.SaveChanges();
    }

    public void Update(Pet pet)
    {
        _context.Pets.Update(pet);
        _context.SaveChanges();
    }

    public void Remove(int id)
    {
        var pet = _context.Pets.Find(id);

        if (pet == null) return;
        _context.Pets.Remove(pet);
        _context.SaveChanges();
    }
}