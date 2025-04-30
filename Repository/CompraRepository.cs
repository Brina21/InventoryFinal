using InventoryFinal.Data;
using InventoryFinal.DTO;
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

        // Obtener Compra por Id
        public async Task<CompraConDetallesDTO> GetCompraDTOById(int id)
        {
            try
            {
                var compra = await _context.Compras
                    .Include(c => c.Cliente)
                    .Include(c => c.Usuario)
                    .Include(c => c.DetalleCompras)
                        .ThenInclude(dc => dc.Producto)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (compra == null)
                {
                    EscribirFichero.Escribir("No se encontró la Compra con el id: " + id);
                    return new CompraConDetallesDTO();
                }

                return new CompraConDetallesDTO
                {
                    Id = compra.Id,
                    FechaCompra = compra.FechaCompra,
                    Total = compra.Total,
                    NombreCliente = compra.Cliente.Nombre,
                    NombreUsuario = compra.Usuario.Nombre,
                    DetalleCompras = compra.DetalleCompras.Select(dc => new DetalleCompraDTO
                    {
                        Id = dc.Id,
                        NombreProducto = dc.Producto.Nombre,
                        Unidades = dc.Cantidad,
                        PrecioUnitario = dc.PrecioUnitario,
                        SubTotal = dc.Total
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                EscribirFichero.Escribir("Error al obtener la Compra por id: " + ex.Message);
                throw;
            }
        }

        // Obtener todas las Compras
        public async Task<List<CompraConDetallesDTO>> GetAllComprasDTO()
        {
            try
            {
                return await _context.Compras
                    .Include(c => c.Cliente)
                    .Include(c => c.Usuario)
                    .Select(c => new CompraConDetallesDTO
                    {
                        Id = c.Id,
                        FechaCompra = c.FechaCompra,
                        Total = c.Total,
                        NombreCliente = c.Cliente.Nombre,
                        NombreUsuario = c.Usuario.Nombre
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                EscribirFichero.Escribir("Error al obtener todas las Compras: " + ex.Message);
                throw;
            }
        }

        // Crear Compra y sus Detalles
        public async Task<Compra> InsertarCompraConDetalle(CompraConDetallesDTO dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var compra = new Compra
                {
                    FechaCompra = dto.FechaCompra,
                    ClienteId = _context.Clientes.FirstOrDefault(c => c.Nombre == dto.NombreCliente)?.Id,
                    UsuarioId = _context.Usuarios.FirstOrDefault(u => u.Nombre == dto.NombreUsuario)?.Id ?? 0,
                    DetalleCompras = new List<DetalleCompra>()
                };

                foreach (var detalle in dto.DetalleCompras)
                {
                    compra.DetalleCompras.Add(new DetalleCompra
                    {
                        CompraId = compra.Id,
                        ProductoId = _context.Productos.FirstOrDefault(p => p.Nombre == detalle.NombreProducto)?.Id,
                        Cantidad = detalle.Unidades,
                        PrecioUnitario = detalle.PrecioUnitario,
                        Total = detalle.SubTotal
                    });

                    // Agregar movimiento de stock
                    var movimientoStock = new MovimientoStock
                    {
                        ProductoId = detalle.Id,
                        Cantidad = detalle.Unidades,
                        TipoMovimiento = Movimiento.Entrada,
                        FechaMovimiento = DateTime.Now,
                        CompraId = compra.Id,
                        UsuarioId = compra.UsuarioId
                    };
                }

                compra.CalcularTotal();

                _context.Compras.Add(compra);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return compra;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                EscribirFichero.Escribir("Error al insertar la Compra: " + ex.Message);
                throw;
            }
        }


        // Actualizar Compra
        public async Task ActualizarCompra(CompraConDetallesDTO dto)
        {
            try
            {
                var compra = await _context.Compras
                    .Include(c => c.DetalleCompras)
                    .FirstOrDefaultAsync(c => c.Id == dto.Id);

                if (compra == null)
                {
                    EscribirFichero.Escribir("No se encontró la Compra con el id: " + dto.Id);
                    return;
                }

                // Actualizar los detalles de la compra
                compra.FechaCompra = dto.FechaCompra;
                compra.Total = dto.Total;
                compra.ClienteId = _context.Clientes.FirstOrDefault(c => c.Nombre == dto.NombreCliente)?.Id;
                compra.UsuarioId = _context.Usuarios.FirstOrDefault(u => u.Nombre == dto.NombreUsuario)?.Id ?? 0;

                // Recuperar y revertir los movimientos de stock
                var movimientos = _context.MovimientoStocks
                    .Where(m => m.CompraId == compra.Id)
                    .ToList();
                foreach (var movimiento in movimientos)
                {
                    var producto = await _context.Productos.FindAsync(movimiento.ProductoId);
                    if (producto != null)
                    {
                        producto.Stock -= movimiento.Cantidad;
                        _context.Productos.Update(producto);
                    }
                }
                // Eliminar los movimientos de stock asociados a la compra
                _context.MovimientoStocks.RemoveRange(movimientos);

                // Eliminar los detalles existentes
                _context.DetalleCompras.RemoveRange(compra.DetalleCompras);

                // Añadir los nuevos detalles
                foreach (var detalle in dto.DetalleCompras)
                {
                    compra.DetalleCompras.Add(new DetalleCompra
                    {
                        ProductoId = _context.Productos.FirstOrDefault(p => p.Nombre == detalle.NombreProducto)?.Id,
                        Cantidad = detalle.Unidades,
                        PrecioUnitario = detalle.PrecioUnitario,
                        Total = detalle.SubTotal
                    });
                }

                // Calcular el nuevo total de la compra
                compra.CalcularTotal();

                // Guardar los cambios en la base de datos
                _context.Compras.Update(compra);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                EscribirFichero.Escribir("Error al actualizar la Compra: " + ex.Message);
                throw;
            }
        }
    }
}