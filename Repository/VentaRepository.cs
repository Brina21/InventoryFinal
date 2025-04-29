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

        // Obtener Venta por Id


    }
}
