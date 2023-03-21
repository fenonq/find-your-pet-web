using DAL.Model;
using DAL.Repository;

namespace BLL.Service.impl;

public class PetService : IPetService
{
    private readonly IEntityRepository<Pet> _petRepository;

    public PetService(IEntityRepository<Pet> petRepository)
    {
        _petRepository = petRepository;
    }

    public void Add(string name, int age, string description, int ownerId)
    {
        var pet = new Pet
        {
            Name = name,
            Age = age,
            Description = description,
            OwnerId = ownerId
        };

        _petRepository.Add(pet);
    }

    public List<Pet> FindAll()
    {
        return _petRepository.FindAll().ToList();
    }

    public Pet? FindById(int id)
    {
        return _petRepository.FindById(id);
    }

    public void Remove(int id)
    {
        _petRepository.Remove(id);
    }
}