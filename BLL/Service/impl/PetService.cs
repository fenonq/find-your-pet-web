namespace BLL.Service.impl;

using DAL.Model;
using DAL.Repository;

public class PetService : IPetService
{
    private readonly IEntityRepository<Pet> _petRepository;

    public PetService(IEntityRepository<Pet> petRepository)
    {
        this._petRepository = petRepository;
    }

    public void Add(string name, int age, string description, int ownerId)
    {
        var pet = new Pet
        {
            Name = name,
            Age = age,
            Description = description,
            OwnerId = ownerId,
        };

        this._petRepository.Add(pet);
    }

    public List<Pet> FindAll()
    {
        return this._petRepository.FindAll().ToList();
    }

    public Pet? FindById(int id)
    {
        return this._petRepository.FindById(id);
    }

    public void Remove(int id)
    {
        this._petRepository.Remove(id);
    }
}