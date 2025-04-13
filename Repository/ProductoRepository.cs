using InventoryFinal.Data;
using InventoryFinal.EscribirLogsFichero;
using InventoryFinal.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryFinal.Repository
{
    public class ProductoRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Obtener Productos por Id
        public async Task<Producto> GetByProductId(int id)
        {
            try
            {
                return await _context.Productos
                    .Include(p => p.Categoria)
                    .Include(p => p.Proveedor)
                    .FirstOrDefaultAsync(p => p.Id == id);
            }
            catch (Exception ex)
            {
                EscribirFichero.Escribir("Error al obtener el Producto por id: " + ex.Message);
                throw;
            }
        }

        // Obtener todos los Productos
        public async Task<List<Producto>> GetAllProductos()
        {
            try
            {
                return await _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Proveedor)
                .ToListAsync();
            }
            catch (Exception ex)
            {
                EscribirFichero.Escribir("Error al obtener los Productos: " + ex.Message);
                throw;
            }
        }
    }
}
