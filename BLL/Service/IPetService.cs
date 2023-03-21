using DAL.Model;

namespace BLL.Service;

public interface IPetService
{
    void Add(string name, int age, string description, int ownerId);

    List<Pet> FindAll();

    Pet? FindById(int id);

    void Remove(int id);
}