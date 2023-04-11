using BLL.Service.impl;
using DAL.Model;
using DAL.Repository;
using Moq;

namespace Tests;

public class ImageServiceTests
{
    private readonly Mock<IImageRepository> _imageRepositoryMock;
    private readonly ImageService _imageService;

    public ImageServiceTests()
    {
        _imageRepositoryMock = new Mock<IImageRepository>();
        _imageService = new ImageService(_imageRepositoryMock.Object);
    }

    [Fact]
    public void FindAll_ReturnsAllImages()
    {
        var images = new List<Image> { new Image { Id = 1 }, new Image { Id = 2 } };
        _imageRepositoryMock.Setup(repo => repo.FindAll()).Returns(images.AsQueryable());

        var result = _imageService.FindAll();

        Assert.Equal(images, result);
    }

    [Fact]
    public void FindById_ReturnsCorrectImage()
    {
        var image = new Image { Id = 1 };
        _imageRepositoryMock.Setup(repo => repo.FindById(1)).Returns(image);

        var result = _imageService.FindById(1);

        Assert.Equal(image, result);
    }

    [Fact]
    public void FindByPetId_ReturnsCorrectImage()
    {
        var images = new List<Image> { new() { Id = 1, PetId = 2 }, new() { Id = 2, PetId = 3 } };
        _imageRepositoryMock.Setup(repo => repo.FindAll()).Returns(images.AsQueryable());

        var result = _imageService.FindByPetId(2);

        Assert.Equal(images.First(), result);
    }

    [Fact]
    public void Add_CallsRepositoryAddMethod()
    {
        var image = new Image { Id = 1 };

        _imageService.Add(image);

        _imageRepositoryMock.Verify(repo => repo.Add(image), Times.Once);
    }

    [Fact]
    public void Remove_CallsRepositoryRemoveMethod()
    {
        var id = 1;

        _imageService.Remove(id);

        _imageRepositoryMock.Verify(repo => repo.Remove(id), Times.Once);
    }
}