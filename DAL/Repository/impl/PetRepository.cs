namespace DAL.Repository.impl;

using DataContext;
using Model;

public class PetRepository : IEntityRepository<Pet>
{
    private readonly FindYourPetContext _context;

    public PetRepository(FindYourPetContext context)
    {
        this._context = context;
    }

    public IQueryable<Pet> FindAll()
    {
        return this._context.Pets;
    }

    public Pet? FindById(int id)
    {
        return this._context.Pets.Find(id);
    }

    public void Add(Pet pet)
    {
        this._context.Pets.Add(pet);
        this._context.SaveChanges();
    }

    public void Update(Pet pet)
    {
        this._context.Pets.Update(pet);
        this._context.SaveChanges();
    }

    public void Remove(int id)
    {
        var pet = this._context.Pets.Find(id);

        if (pet == null) return;
        this._context.Pets.Remove(pet);
        this._context.SaveChanges();
    }
}