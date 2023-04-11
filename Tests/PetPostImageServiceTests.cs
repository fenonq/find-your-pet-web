using BLL.Dto;
using BLL.Service;
using BLL.Service.impl;
using DAL.Model;
using DAL.Model.Enum;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Tests;

public class PetPostImageServiceTests
{
    private readonly PetPostImageService _petPostImageService;
    private readonly Mock<IPostService> _mockPostService;
    private readonly Mock<IPetService> _mockPetService;
    private readonly Mock<IImageService> _mockImageService;
    private readonly Mock<IWebHostEnvironment> _mockWebHostEnvironment;

    public PetPostImageServiceTests()
    {
        _mockPostService = new Mock<IPostService>();
        _mockPetService = new Mock<IPetService>();
        _mockImageService = new Mock<IImageService>();
        _mockWebHostEnvironment = new Mock<IWebHostEnvironment>();

        _petPostImageService = new PetPostImageService(
            _mockPostService.Object,
            _mockPetService.Object,
            _mockImageService.Object,
            _mockWebHostEnvironment.Object);
    }

    [Fact]
    public void AddPetPostImage_WithValidInputs_ReturnsPostId()
    {
        var post = new Post();
        var pet = new Pet();
        var formFile = new Mock<IFormFile>();
        formFile.Setup(f => f.Length).Returns(1);
        var webRootPath = "../../../../PresentationLayer/wwwroot";
        var expectedPostId = 1;

        _mockPetService.Setup(p => p.Add(pet)).Returns(pet.Id);
        _mockPostService.Setup(p => p.Add(post)).Returns(expectedPostId);
        _mockWebHostEnvironment.Setup(w => w.WebRootPath).Returns(webRootPath);

        var result = _petPostImageService.AddPetPostImage(post, pet, 1, formFile.Object);

        Assert.Equal(expectedPostId, result);
        _mockImageService.Verify(i => i.Add(It.IsAny<Image>()), Times.Once);
        _mockPetService.Verify(i => i.Add(It.IsAny<Pet>()), Times.Once);
        _mockPostService.Verify(i => i.Add(It.IsAny<Post>()), Times.Once);
    }

    [Fact]
    public void AddPetPostImage_WithInvalidFormFile_ReturnsPostIdWithoutAddingImage()
    {
        var post = new Post();
        var pet = new Pet();
        var formFile = new Mock<IFormFile>();
        formFile.Setup(f => f.Length).Returns(0);
        var expectedPostId = 0;

        _mockPetService.Setup(p => p.Add(pet)).Returns(pet.Id);
        _mockPostService.Setup(p => p.Add(post)).Returns(expectedPostId);

        var result = _petPostImageService.AddPetPostImage(post, pet, 1, formFile.Object);

        Assert.Equal(expectedPostId, result);
        _mockImageService.Verify(i => i.Add(It.IsAny<Image>()), Times.Never);
    }

    [Fact]
    public void FindAllPetPostImage_WithValidSortOrder_ReturnsSortedPetPostImageDtos()
    {
        var sortOrder = "lost_date";
        var posts = new List<Post>
        {
            new()
            {
                Id = 1, IsActive = true, Date = new DateOnly(2022, 1, 1), Location = "A", Type = PostType.Lost,
                PetId = 1
            },
            new()
            {
                Id = 2, IsActive = true, Date = new DateOnly(2022, 1, 2), Location = "B", Type = PostType.Found,
                PetId = 2
            },
            new()
            {
                Id = 3, IsActive = true, Date = new DateOnly(2022, 1, 3), Location = "C", Type = PostType.Lost,
                PetId = 3
            },
            new()
            {
                Id = 4, IsActive = true, Date = new DateOnly(2022, 1, 4), Location = "D", Type = PostType.Found,
                PetId = 4
            }
        };
        var images = new List<Image>
        {
            new() { Id = 1, Path = "/uploads/test1.jpg", PetId = 1 },
            new() { Id = 2, Path = "/uploads/test2.jpg", PetId = 2 },
            new() { Id = 3, Path = "/uploads/test3.jpg", PetId = 3 },
            new() { Id = 4, Path = "/uploads/test4.jpg", PetId = 4 }
        };
        var pets = new List<Pet>
        {
            new() { Id = 1, Name = "1", Age = 1, Description = "1" },
            new() { Id = 2, Name = "2", Age = 2, Description = "2" },
            new() { Id = 3, Name = "3", Age = 3, Description = "3" },
            new() { Id = 4, Name = "4", Age = 4, Description = "4" }
        };
        var expectedDto = new List<PetPostImageDto>
        {
            new()
            {
                Post = posts[3],
                Pet = pets[3],
                Image = images[3]
            },
            new()
            {
                Post = posts[2],
                Pet = pets[2],
                Image = images[2]
            },
            new()
            {
                Post = posts[1],
                Pet = pets[1],
                Image = images[1]
            },
            new()
            {
                Post = posts[0],
                Pet = pets[0],
                Image = images[0]
            }
        };

        _mockPostService.Setup(p => p.FindAll()).Returns(posts);
        _mockPetService.Setup(p => p.FindAll()).Returns(pets);
        _mockImageService.Setup(i => i.FindAll()).Returns(images);

        var result = _petPostImageService.FindAllPetPostImage(sortOrder);

        Assert.Equal(expectedDto[0].Post, result.ToList()[0].Post);
    }

    [Fact]
    public void FindByPostId_WithValidPostId_ReturnsPetPostImageDto()
    {
        var postId = 1;
        var post = new Post { Id = postId, PetId = 1 };
        var pet = new Pet { Id = 1 };
        var image = new Image { Id = 1, PetId = 1 };

        _mockPostService.Setup(p => p.FindById(postId)).Returns(post);
        _mockPetService.Setup(p => p.FindById(post.PetId)).Returns(pet);
        _mockImageService.Setup(i => i.FindByPetId(post.PetId)).Returns(image);

        var result = _petPostImageService.FindByPostId(postId);

        Assert.NotNull(result);
        Assert.Equal(post, result.Post);
        Assert.Equal(pet, result.Pet);
        Assert.Equal(image, result.Image);
    }
}