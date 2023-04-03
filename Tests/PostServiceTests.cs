using BLL.Service.impl;
using DAL.Model;
using DAL.Model.Enum;
using DAL.Repository;
using Moq;

namespace Tests;

public class PostServiceTests
{
    private readonly Mock<IPostRepository> _postRepositoryMock;
    private readonly PostService _postService;

    public PostServiceTests()
    {
        _postRepositoryMock = new Mock<IPostRepository>();
        _postService = new PostService(_postRepositoryMock.Object);
    }

    [Fact]
    public void Add_CallsPostRepositoryAddMethod()
    {
        var post = new Post();

        _postService.Add(post);

        _postRepositoryMock.Verify(repo => repo.Add(post), Times.Once);
    }

    [Fact]
    public void Add_ReturnsPostId()
    {
        var post = new Post { Id = 1 };
        _postRepositoryMock.Setup(repo => repo.Add(post));

        var result = _postService.Add(post);

        Assert.Equal(post.Id, result);
    }

    [Fact]
    public void Add_SetsCreatedAtAndIsActiveFields()
    {
        var post = new Post();
        _postService.Add(post);
        Assert.True(post.IsActive);
    }

    [Fact]
    public void ChangeVisibility_CallsPostRepositoryUpdateMethod()
    {
        var post = new Post { Id = 1, IsActive = true };
        _postRepositoryMock.Setup(repo => repo.FindById(post.Id)).Returns(post);

        _postService.ChangeVisibility(post.Id);

        _postRepositoryMock.Verify(repo => repo.Update(post), Times.Once);
    }

    [Fact]
    public void ChangeVisibility_ChangesPostVisibility()
    {
        var post = new Post { Id = 1, IsActive = true };
        _postRepositoryMock.Setup(repo => repo.FindById(post.Id)).Returns(post);

        _postService.ChangeVisibility(post.Id);

        Assert.False(post.IsActive);
    }

    [Fact]
    public void ChangeVisibility_InvalidPostId_DoesNothing()
    {
        var postId = 1;
        _postRepositoryMock.Setup(repo => repo.FindById(postId)).Returns<Post>(null);

        _postService.ChangeVisibility(postId);

        _postRepositoryMock.Verify(repo => repo.Update(It.IsAny<Post>()), Times.Never);
    }

    [Fact]
    public void FindAll_ReturnsListOfPosts()
    {
        var posts = new List<Post> { new(), new() };
        _postRepositoryMock.Setup(repo => repo.FindAll()).Returns(posts.AsQueryable());

        var result = _postService.FindAll();

        Assert.Equal(posts, result);
    }

    [Fact]
    public void FindAllByUserId_ReturnsListOfPostsByUserId()
    {
        const int userId = 1;
        var posts = new List<Post> { new() { UserId = userId }, new() { UserId = 2 } };
        _postRepositoryMock.Setup(repo => repo.FindAll()).Returns(posts.AsQueryable());

        var result = _postService.FindAllByUserId(userId);

        Assert.Equal(posts.Where(p => p.UserId == userId).ToList(), result);
    }

    [Fact]
    public void FindAllByPostType_ReturnsListOfPostsByPostType()
    {
        const int postType = 1;
        var posts = new List<Post> { new() { Type = (PostType)postType }, new() { Type = PostType.Found } };
        _postRepositoryMock.Setup(repo => repo.FindAll()).Returns(posts.AsQueryable());

        var result = _postService.FindAllByPostType(postType);

        Assert.Equal(posts.Where(p => (int)p.Type == postType).ToList(), result);
    }

    [Fact]
    public void FindById_ReturnsPostById()
    {
        var post = new Post { Id = 1 };
        _postRepositoryMock.Setup(repo => repo.FindById(1)).Returns(post);

        var result = _postService.FindById(1);

        Assert.Equal(post, result);
    }

    [Fact]
    public void Remove_CallsPostRepositoryRemoveMethod()
    {
        const int postId = 1;

        _postService.Remove(postId);
        _postRepositoryMock.Verify(repo => repo.Remove(postId), Times.Once);
    }
}