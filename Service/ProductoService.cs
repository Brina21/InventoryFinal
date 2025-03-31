using InventoryFinal.Data;
using InventoryFinal.EscribirLogsFichero;
using InventoryFinal.Models;
using InventoryFinal.Repository;
using Microsoft.IdentityModel.Tokens;

namespace InventoryFinal.Service
{
    public class ProductoService : GenericoService<Producto>
    {
        private readonly ProductoRepository productoRepository;

        public ProductoService(ProductoRepository repository) : base(repository)
        {
            productoRepository = repository;
        }

        public async Task<(bool, string mensaje, Producto? producto)> ObtenerProductoPorId(int id)
        {
            var entidad = await productoRepository.GetByProductId(id);

            if (entidad == null)
            {
                EscribirFichero.Escribir($"No se ha podido obtener {entidad?.Nombre}.");
                return (false, $"No se ha encontrado el producto {entidad?.Nombre}", null);
            }

            EscribirFichero.Escribir($"Se ha encontrado {entidad?.Nombre}.");
            return (true, $"Se ha encontrado {entidad?.Nombre}", entidad);
        }

        public async Task<(bool exito, string mensaje, List<Producto> productos)> ObtenerTodosProductos()
        {
            List<Producto> entidades = await productoRepository.GetAllProductos();

            if (!entidades.IsNullOrEmpty())
            {
                EscribirFichero.Escribir("Productos obtenidos correctamente");
                return (true, "Productos obtenidos correctamente", entidades);
            }

            return (false, "No se encontraron productos", new List<Producto>());
        }
    }
}
