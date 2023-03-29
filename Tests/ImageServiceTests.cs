
using BLL.Service.impl;
using DAL.DataContext;
using DAL.Model;
using DAL.Repository.impl;
using EntityFrameworkCoreMock;

namespace Tests
{
    public class ImageServiceTests
    {

        ImageService createImageService()
        {
            var dbContextMock = new DbContextMock<FindYourPetContext>();
            dbContextMock.CreateDbSetMock(x => x.Images, new List<Image>());
            var repository = new ImageRepository(dbContextMock.Object);
            return new ImageService(repository);
        }

        [Fact]
        public void Add_AddImage_ImageServiceShouldContainOnlyOneImage()
        {
            var imageSrvice = createImageService();

            Image image = new Image();
            image.Id = 0;

            imageSrvice.Add(image);

            var allImages = imageSrvice.FindAll();
            Assert.Single(allImages);

            var firstImage = imageSrvice.FindById(0);
            Assert.Equal(firstImage, image);
        }

        [Fact]
        public void Remove_RemoveImage_ImageServiceShouldSuccessfullyRemoveImage()
        {
            var imageService = createImageService();

            Image image1 = new Image();
            image1.Id = 0;

            Image image2 = new Image();
            image2.Id = 1;

            imageService.Add(image1);
            imageService.Add(image2);

            imageService.Remove(1);

            var allImages = imageService.FindAll();

            Assert.Single(allImages);
            Assert.Equal(allImages[0], image1);
        }

        [Fact]
        public void FindAll_AddSomeImages_FindAllShouldReturnCorrectResult()
        {
            var imageService = createImageService();

            var images = new List<Image>();
            //Add ten images
            foreach (var i in Enumerable.Range(0, 10))
            {
                var image = new Image();
                image.Id = i;

                images.Add(image);
                imageService.Add(image);
            }

            Assert.Equal(images, imageService.FindAll());
        }

        [Fact]
        public void FindById_AddSomeImages_FindByIdShouldReturnCorrectResult()
        {
            var imageService = createImageService();

            var images = new List<Image>();
            //Add ten images
            foreach (var i in Enumerable.Range(0, 10))
            {
                var image = new Image();
                image.Id = i;

                images.Add(image);
                imageService.Add(image);
            }

            var expectedImage = new Image();
            expectedImage.Id = 11;

            imageService.Add(expectedImage);

            Assert.Equal(expectedImage, imageService.FindById(11));
        }

    }
}
