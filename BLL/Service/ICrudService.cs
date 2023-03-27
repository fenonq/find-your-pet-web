namespace BLL.Service;

public interface ICrudService<T>
{
    List<T> FindAll();

    T? FindById(int id);

    int Add(T obj);

    void Remove(int id);
}