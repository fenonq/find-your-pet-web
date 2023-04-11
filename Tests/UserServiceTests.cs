// using BLL.Service.impl;
// using DAL.Model;
// using DAL.Repository;
// using Moq;
//
// namespace Tests;
//
// public class UserServiceTests
// {
//     private readonly Mock<IUserRepository> _userRepositoryMock;
//     private readonly UserService _userService;
//
//     public UserServiceTests()
//     {
//         _userRepositoryMock = new Mock<IUserRepository>();
//         _userService = new UserService(_userRepositoryMock.Object);
//     }
//
//     [Fact]
//     public void FindAll_ReturnsListOfUsers()
//     {
//         var users = new List<User> { new(), new() };
//         _userRepositoryMock.Setup(repo => repo.FindAll()).Returns(users.AsQueryable());
//
//         var result = _userService.FindAll();
//
//         Assert.Equal(users, result);
//     }
//
//     [Fact]
//     public void FindById_ReturnsUserById()
//     {
//         var user = new User { Id = 1 };
//         _userRepositoryMock.Setup(repo => repo.FindById(1)).Returns(user);
//
//         var result = _userService.FindById(1);
//
//         Assert.Equal(user, result);
//     }
//
//     [Fact]
//     public void Add_CallsUserRepositoryAddMethod()
//     {
//         var user = new User { Name = "John", Login = "johndoe", Password = "password" };
//
//         _userService.Add(user);
//
//         _userRepositoryMock.Verify(repo => repo.Add(user), Times.Once);
//     }
//
//     [Fact]
//     public void Add_ReturnsUserId()
//     {
//         var user = new User { Name = "John", Login = "johndoe", Password = "password", Id = 1 };
//         _userRepositoryMock.Setup(repo => repo.Add(user));
//
//         var result = _userService.Add(user);
//
//         Assert.Equal(user.Id, result);
//     }
//
//     [Fact]
//     public void LoginUser_ReturnsTrueIfCredentialsAreValid()
//     {
//         var user = new User { Login = "johndoe", Password = "password" };
//         _userRepositoryMock.Setup(repo => repo.FindAll()).Returns(new List<User> { user }.AsQueryable());
//
//         var result = _userService.LoginUser("johndoe", "password");
//
//         Assert.True(result);
//     }
//
//     [Fact]
//     public void LoginUser_ReturnsFalseIfUserDoesNotExist()
//     {
//         _userRepositoryMock.Setup(repo => repo.FindAll()).Returns(new List<User>().AsQueryable());
//
//         var result = _userService.LoginUser("johndoe", "password");
//
//         Assert.False(result);
//     }
//
//     [Fact]
//     public void LoginUser_ReturnsFalseIfPasswordIsIncorrect()
//     {
//         var user = new User { Login = "johndoe", Password = "password" };
//         _userRepositoryMock.Setup(repo => repo.FindAll()).Returns(new List<User> { user }.AsQueryable());
//
//         var result = _userService.LoginUser("johndoe", "wrongpassword");
//
//         Assert.False(result);
//     }
//
//     [Fact]
//     public void Remove_CallsUserRepositoryRemoveMethod()
//     {
//         const int userId = 1;
//
//         _userService.Remove(userId);
//
//         _userRepositoryMock.Verify(repo => repo.Remove(userId), Times.Once);
//     }
// }