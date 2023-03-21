namespace DAL.Repository
{
    public interface IEntityRepository<T>
    {
        T? FindById(int id);

        void Add(T entity);
        
        void Update(T entity);

        void Remove(int id);

        IQueryable<T> FindAll();
    }
}