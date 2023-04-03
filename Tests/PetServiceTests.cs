using BLL.Service.impl;
using DAL.Model;
using DAL.Repository;
using Moq;

namespace Tests;

public class PetServiceTests
{
    private readonly Mock<IPetRepository> _petRepositoryMock;
    private readonly PetService _petService;

    public PetServiceTests()
    {
        _petRepositoryMock = new Mock<IPetRepository>();
        _petService = new PetService(_petRepositoryMock.Object);
    }

    [Fact]
    public void Add_CallsPetRepositoryAddMethod()
    {
        var pet = new Pet { Name = "Fluffy" };

        _petService.Add(pet);

        _petRepositoryMock.Verify(repo => repo.Add(pet), Times.Once);
    }

    [Fact]
    public void Add_ReturnsPetId()
    {
        var pet = new Pet { Name = "Fluffy", Id = 1 };
        _petRepositoryMock.Setup(repo => repo.Add(pet));

        var result = _petService.Add(pet);

        Assert.Equal(pet.Id, result);
    }

    [Fact]
    public void Add_WhenNameIsNull_SetsNameToDash()
    {
        var pet = new Pet { Name = null };

        _petService.Add(pet);

        Assert.Equal("-", pet.Name);
    }

    [Fact]
    public void FindAll_ReturnsListOfPets()
    {
        var pets = new List<Pet> { new(), new() };
        _petRepositoryMock.Setup(repo => repo.FindAll()).Returns(pets.AsQueryable());

        var result = _petService.FindAll();

        Assert.Equal(pets, result);
    }

    [Fact]
    public void FindById_ReturnsPetById()
    {
        var pet = new Pet { Id = 1 };
        _petRepositoryMock.Setup(repo => repo.FindById(1)).Returns(pet);

        var result = _petService.FindById(1);

        Assert.Equal(pet, result);
    }

    [Fact]
    public void Remove_CallsPetRepositoryRemoveMethod()
    {
        const int petId = 1;

        _petService.Remove(petId);

        _petRepositoryMock.Verify(repo => repo.Remove(petId), Times.Once);
    }
}
