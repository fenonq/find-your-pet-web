namespace BLL.Service;

using DAL.Model;

public interface IPetService
{
    void Add(string name, int age, string description, int ownerId);

    List<Pet> FindAll();

    Pet? FindById(int id);

    void Remove(int id);
}