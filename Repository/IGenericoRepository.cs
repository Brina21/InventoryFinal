namespace InventoryFinal.Repository
{
    public interface IGenericoRepository<T>
    {
        Task<T> Insert(T entity);
        Task<T> GetById(int id);
        Task<List<T>> GetAll();
        Task<bool> Update(T entity);
        Task<bool> Delete(int id);
    }
}
