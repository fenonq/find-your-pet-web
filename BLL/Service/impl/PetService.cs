namespace BLL.Service.impl;

using DAL.Model;
using DAL.Repository;

public class PetService : IPetService
{
    private readonly IPetRepository _petRepository;

    public PetService(IPetRepository petRepository)
    {
        _petRepository = petRepository;
    }

    public int Add(Pet pet)
    {
        pet.Name = string.IsNullOrEmpty(pet.Name) ? "-" : pet.Name;
        _petRepository.Add(pet);
        return pet.Id;
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