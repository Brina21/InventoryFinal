using InventoryFinal.Data;
using InventoryFinal.EscribirLogsFichero;
using InventoryFinal.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryFinal.Repository
{
    public class VentaRepository
    {
        private readonly ApplicationDbContext _context;

        public VentaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Obtener Ventas por Id
        public async Task<Venta> GetByVentaId(int id)
        {
            try
            {
                return await _context.Ventas
                    .Include(v => v.Cliente)
                    .Include(v => v.Usuario)
                    .FirstOrDefaultAsync(v => v.Id == id);
            }
            catch (Exception ex)
            {
                EscribirFichero.Escribir("Error la obtener la Venta por Id: " + ex.Message);
                throw;
            }
        }

        // Obtener todas las Ventas
        public async Task<List<Venta>> GetAllVentas()
        {
            try
            {
                return await _context.Ventas
                    .Include(v => v.Cliente)
                    .Include(v => v.Usuario)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                EscribirFichero.Escribir("Error la obtener las Venta: " + ex.Message);
                throw;
            }
        }
    }
}
