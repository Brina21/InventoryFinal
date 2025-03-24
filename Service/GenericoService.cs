using InventoryFinal.EscribirLogsFichero;
using InventoryFinal.Repository;
using Microsoft.IdentityModel.Tokens;

namespace InventoryFinal.Service
{
    public class GenericoService<T> : IGenericoService<T> where T : class
    {
        private readonly IGenericoRepository<T> genericoRepository;

        public GenericoService(IGenericoRepository<T> genericoRepository)
        {
            this.genericoRepository = genericoRepository;
        }

        public async Task<(bool exito, string mensaje)> Crear(T entidad)
        {
            T? entidadResultado = await genericoRepository.Insert(entidad);

            if (entidadResultado != null)
            {
                EscribirFichero.Escribir($"{typeof(T).Name} creada correctamente.");

                return (true, $"{typeof(T).Name} creada correctamente.");
            }

            EscribirFichero.Escribir($"Error al crear: {typeof(T).Name}.");
            return (false, $"Error al crear: {typeof(T).Name}.");
        }

        public async Task<(bool exito, string mensaje, T? resultado)> ObtenerPorId(int id)
        {
            T? resultado = await genericoRepository.GetById(id);

            if (resultado != null)
            {
                EscribirFichero.Escribir($"{typeof(T).Name} obtenida correctamente.");
                return (true, $"{typeof(T).Name} obtenida correctamente.", resultado);
            }

            EscribirFichero.Escribir($"{typeof(T).Name} no se ha obtenido.");
            return (false, $"{typeof(T).Name} no se ha obtenido", null);
        }

        public async Task<(bool exito, string mensaje, List<T> entidades)> ObtenerTodos()
        {
            List<T> entidades = await genericoRepository.GetAll();

            if (!entidades.IsNullOrEmpty())
            {
                EscribirFichero.Escribir($"Entidades de {typeof(T).Name} obtenidas correctamente.");
                return (true, $"Entidades de {typeof(T).Name} obtenidas correctamente.", entidades);
            }

            EscribirFichero.Escribir($"Entidades de {typeof(T).Name} no se han obtenido.");
            return (false, $"No se han encontrado resultados", new List<T>());
        }

        public async Task<(bool exito, string mensaje)> Actualizar(T entidad)
        {
            bool resultado = await genericoRepository.Update(entidad);

            if (resultado == true)
            {
                EscribirFichero.Escribir($"{typeof(T).Name} actualizada correctamente.");
                return (true, $"{typeof(T).Name} actualizada correctamente.");
            }

            EscribirFichero.Escribir($"{typeof(T).Name} no se ha podido actualizar.");
            return (true, $"{typeof(T).Name} no se ha podido actualizar.");
        }

        public async Task<(bool exito, string mensaje)> Eliminar(int id)
        {
            bool resultado = await genericoRepository.Delete(id);

            if (resultado == true)
            {
                EscribirFichero.Escribir($"{typeof(T).Name} eliminada correctamente.");
                return (true, $"{typeof(T).Name} eliminada correctamente.");
            }

            EscribirFichero.Escribir($"{typeof(T).Name} no se ha podido eliminar.");
            return (true, $"{typeof(T).Name} no se ha podido eliminar.");
        }
    }
}
