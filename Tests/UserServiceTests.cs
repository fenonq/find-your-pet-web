using BLL.Service.impl;
using DAL.DataContext;
using DAL.Model;
using DAL.Repository.impl;
using EntityFrameworkCoreMock;

namespace Tests
{
    public class UserServiceTests
    {

        UserService createUserService()
        {
            var dbContextMock = new DbContextMock<FindYourPetContext>();
            dbContextMock.CreateDbSetMock(x => x.Users, new List<User>());
            UserRepository repository = new UserRepository(dbContextMock.Object);
            return new UserService(repository);
        }

        [Fact]
        public void Add_AddSingleUser_UserServiceShouldContainOnlyOneUser()
        {
            var userService = createUserService();

            User user1 = new User();
            user1.Name = "Test User";
            user1.Id = 0;

            userService.Add(user1);

            var allUsers = userService.FindAll();
            Assert.Single(allUsers);

            var firstUser =  userService.FindById(0);
            Assert.Equal(firstUser, user1);
        }

        [Fact]
        public void Remove_RemoveUser_UserServiceShouldSuccessfullyRemoveUser()
        {
            var userService = createUserService();

            User user1 = new User();
            user1.Name = "Test User1";
            user1.Id = 0;

            User user2 = new User();
            user2.Name = "Test User2";
            user2.Id = 1;

            userService.Add(user1);
            userService.Add(user2);

            userService.Remove(1);

            var allUsers = userService.FindAll();

            Assert.Single(allUsers);
            Assert.Equal(allUsers[0], user1);
        }

        [Fact]
        public void LoginUser_UserWithCorrectPassword_UserShouldSuccessfullyLogin()
        {
            var userService = createUserService();

            var login = "test@gmail.com";
            var pass = "1111";

            User user1 = new User();
            user1.Name = "Test User";
            user1.Id = 0;
            user1.Login = login;
            user1.Password = pass;

            userService.Add(user1);

            Assert.True(userService.LoginUser(login, pass));
        }

        [Fact]
        public void LoginUser_UserWithInCorrectPassword_UserShouldFailedLogin()
        {
            var userService = createUserService();

            var login = "test@gmail.com";
            var pass = "1111";

            User user1 = new User();
            user1.Name = "Test User";
            user1.Id = 0;
            user1.Login = login;
            user1.Password = "another_pass";

            userService.Add(user1);

            Assert.False(userService.LoginUser(login, pass));
        }


        [Fact]
        public void FindAll_AddSomeUsers_FindAllShouldReturnCorrectResult()
        {
            var userService = createUserService();

            var users = new List<User>();
            //Add ten users
            foreach(var i in Enumerable.Range(0, 10))
            {
                var user = new User();
                user.Id = i;
                user.Name = "Test Name";;

                users.Add(user);
                userService.Add(user);
            }

            Assert.Equal(users, userService.FindAll());
        }

        [Fact]
        public void FindById_AddSomeUsers_FindByIdShouldReturnCorrectResult()
        {
            var userService = createUserService();

            var users = new List<User>();
            //Add ten users
            foreach (var i in Enumerable.Range(0, 10))
            {
                var user = new User();
                user.Id = i;
                user.Name = "Test Name";

                users.Add(user);
                userService.Add(user);
            }

            var expectedUser = new User();
            expectedUser.Id = 11;
            expectedUser.Name = "Expected User";

            userService.Add(expectedUser);

            Assert.Equal(expectedUser, userService.FindById(11));
        }
    }
}