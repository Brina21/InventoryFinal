namespace InventoryFinal.Service
{
    public interface IGenericoService<T>
    {
        Task<(bool exito, string mensaje, T? resultado)> Crear(T entidad);
        Task<(bool exito, string mensaje, T? resultado)> ObtenerPorId(int id);
        Task<(bool exito, string mensaje, List<T> entidades)> ObtenerTodos();
        Task<(bool exito, string mensaje)> Actualizar(T entidad);
        Task<(bool exito, string mensaje)> Eliminar(int id);
    }
}
