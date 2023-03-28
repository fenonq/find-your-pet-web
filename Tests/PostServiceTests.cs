using BLL.Service.impl;
using DAL.DataContext;
using DAL.Model;
using DAL.Model.Enum;
using DAL.Repository.impl;
using EntityFrameworkCoreMock;

namespace Tests
{
    public class PostServiceTests
    {
        PostService createPostService()
        {
            var dbContextMock = new DbContextMock<FindYourPetContext>();
            dbContextMock.CreateDbSetMock(x => x.Posts, new List<Post>());
            var repository = new PostRepository(dbContextMock.Object);
            return new PostService(repository);
        }

        [Fact]
        public void Add_AddPost_PostServiceShouldContainOnlyOnePost()
        {
            var postSrvice = createPostService();

            Post post = new Post();
            post.Id = 0;

            postSrvice.Add(post);

            var allPosts = postSrvice.FindAll();
            Assert.Single(allPosts);

            var firstPost = postSrvice.FindById(0);
            Assert.Equal(firstPost, post);
        }

        [Fact]
        public void FindAll_AddSomePosts_FindAllShouldReturnCorrectResult()
        {
            var postService = createPostService();

            var posts = new List<Post>();
            //Add ten posts
            foreach (var i in Enumerable.Range(0, 10))
            {
                var post = new Post();
                post.Id = i;

                posts.Add(post);
                postService.Add(post);
            }

            Assert.Equal(posts, postService.FindAll());
        }

        [Fact]
        public void FindById_AddSomePosts_FindByIdShouldReturnCorrectResult()
        {
            var postService = createPostService();

            var posts = new List<Post>();
            //Add ten posts
            foreach (var i in Enumerable.Range(0, 10))
            {
                var post = new Post();
                post.Id = i;

                posts.Add(post);
                postService.Add(post);
            }

            var expectedPost = new Post();
            expectedPost.Id = 11;

            postService.Add(expectedPost);

            Assert.Equal(expectedPost, postService.FindById(11));
        }

        [Fact]
        public void Remove_RemovePost_PostServiceShouldSuccessfullyRemovePost()
        {
            var postService = createPostService();

            Post post1 = new Post();
            post1.Id = 0;

            Post post2 = new Post();
            post2.Id = 1;

            postService.Add(post1);
            postService.Add(post2);

            postService.Remove(1);

            var allPosts = postService.FindAll();

            Assert.Single(allPosts);
            Assert.Equal(allPosts[0], post1);
        }

        [Fact]
        public void FindAllByUserId_AddSomePosts_FindAllByUserIdShouldReturnCorrectResult()
        {
            var postService = createPostService();

            var posts = new List<Post>();
            //Add ten posts
            foreach (var i in Enumerable.Range(0, 10))
            {
                var post = new Post();
                post.Id = i;
                post.UserId = i;

                posts.Add(post);
                postService.Add(post);
            }

            var expectedPost = new Post();
            expectedPost.Id = 11;
            expectedPost.UserId = 11;

            postService.Add(expectedPost);

            var allForUser11 = postService.FindAllByUserId(11);

            Assert.Single(allForUser11);
            Assert.Equal(expectedPost, allForUser11[0]);
        }

        [Fact]
        public void FindAllByPostType_AddSomePosts_FindAllByPostTypeShouldReturnCorrectResult()
        {
            var postService = createPostService();

            var lostPosts = new List<Post>();
            //Add ten posts with different types
            foreach (var i in Enumerable.Range(0, 10))
            {
                var post = new Post();
                post.Id = i;
                if (i % 2 == 0)
                {
                    post.Type = PostType.Lost;
                    lostPosts.Add(post);
                }
                else
                    post.Type = PostType.Found;

                postService.Add(post);
            }

            Assert.Equal(lostPosts, postService.FindAllByPostType((int)PostType.Lost));
        }

        [Fact]
        public void ChangeVisibility_TryToChangeVisibility_VisibilityShouldSuccessfullyChange()
        {
            var postService = createPostService();

            Post post = new Post();
            post.Id = 0;
            post.IsActive = true;

            postService.Add(post);

            postService.ChangeVisibility(0);

            var firstPost = postService.FindById(0)!;
            Assert.False(firstPost.IsActive);
        }
    }
}
