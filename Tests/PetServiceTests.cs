using BLL.Service.impl;
using DAL.DataContext;
using DAL.Model;
using DAL.Repository.impl;
using EntityFrameworkCoreMock;

namespace Tests
{
    public class PetServiceTests
    {
        PetService createPetService()
        {
            var dbContextMock = new DbContextMock<FindYourPetContext>();
            dbContextMock.CreateDbSetMock(x => x.Pets, new List<Pet>());
            var repository = new PetRepository(dbContextMock.Object);
            return new PetService(repository);
        }

        [Fact]
        public void Add_AddPet_PetServiceShouldContainOnlyOnePet()
        {
            var petSrvice = createPetService();

            Pet pet = new Pet();
            pet.Name = "Test Pet";
            pet.Id = 0;

            petSrvice.Add(pet);

            var allPets = petSrvice.FindAll();
            Assert.Single(allPets);

            var firstPet = petSrvice.FindById(0);
            Assert.Equal(firstPet, pet);
        }

        [Fact]
        public void Remove_RemovePet_PetServiceShouldSuccessfullyRemovePet()
        {
            var petService = createPetService();

            Pet pet1 = new Pet();
            pet1.Name = "Test Pet1";
            pet1.Id = 0;

            Pet pet2 = new Pet();
            pet2.Name = "Test Pet2";
            pet2.Id = 1;

            petService.Add(pet1);
            petService.Add(pet2);

            petService.Remove(1);

            var allPets = petService.FindAll();

            Assert.Single(allPets);
            Assert.Equal(allPets[0], pet1);
        }

        [Fact]
        public void FindAll_AddSomePets_FindAllShouldReturnCorrectResult()
        {
            var petService = createPetService();

            var pets = new List<Pet>();
            //Add ten pets
            foreach (var i in Enumerable.Range(0, 10))
            {
                var pet = new Pet();
                pet.Id = i;
                pet.Name = "Test Name"; ;

                pets.Add(pet);
                petService.Add(pet);
            }

            Assert.Equal(pets, petService.FindAll());
        }

        [Fact]
        public void FindById_AddSomePets_FindByIdShouldReturnCorrectResult()
        {
            var petService = createPetService();

            var pets = new List<Pet>();
            //Add ten pets
            foreach (var i in Enumerable.Range(0, 10))
            {
                var pet = new Pet();
                pet.Id = i;
                pet.Name = "Test Name";

                pets.Add(pet);
                petService.Add(pet);
            }

            var expectedPet = new Pet();
            expectedPet.Id = 11;
            expectedPet.Name = "Expected Pet";

            petService.Add(expectedPet);

            Assert.Equal(expectedPet, petService.FindById(11));
        }
    }
}
