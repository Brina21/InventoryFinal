using InventoryFinal.Data;
using InventoryFinal.EscribirLogsFichero;
using InventoryFinal.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryFinal.Repository
{
    public class CompraRepository : GenericoRepository<Compra>
    {
        private readonly ApplicationDbContext _context;

        public CompraRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        // Obtener Categoría por Id
        public async Task<Compra> GetCompraId(int id)
        {
            try
            {
                return await _context.Compras
                    .Include(c => c.Cliente)
                    .Include(c => c.Usuario)
                    .FirstOrDefaultAsync(c => c.Id == id);
            }
            catch (Exception ex)
            {
                EscribirFichero.Escribir("Error al obtener la Categoria por id: " + ex.Message);
                throw;
            }
        }

        // Obtener todas las Categorías
        public async Task<List<Compra>> GetAllCompra()
        {
            try
            {
                return await _context.Compras
                    .Include(c => c.Cliente)
                    .Include(c => c.Usuario)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                EscribirFichero.Escribir("Error al obtener las Categorías: " + ex.Message);
                throw;
            }
        }
    }
}
