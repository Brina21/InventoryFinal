using InventoryFinal.EscribirLogsFichero;
using InventoryFinal.Models;
using InventoryFinal.Repository;
using Microsoft.IdentityModel.Tokens;

namespace InventoryFinal.Service
{
    public class ProductoService
    {
        private readonly ProductoRepository productoRepository;

        public ProductoService(ProductoRepository repository)
        {
            productoRepository = repository;
        }

        public async Task<(bool exito, string mensaje, Producto? producto)> ObtenerProductoPorId(int id)
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
            List<Producto> resultado = await productoRepository.GetAllProductos();

            if (!resultado.IsNullOrEmpty())
            {
                return (true, "Productos obtenidos correctamente", resultado);
            }

            EscribirFichero.Escribir("No se han obtenido los productos");
            return (false, "No se encontraron productos", new List<Producto>());
        }
    }
}
