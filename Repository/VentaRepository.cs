﻿using InventoryFinal.Data;
using InventoryFinal.DTO;
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
        public async Task<VentaConDetallesDTO> GetVentaDTOById(int id)
        {
            try
            {
                var venta = await _context.Ventas
                    .Include(v => v.Cliente)
                    .Include(v => v.Usuario)
                    .Include(v => v.DetalleVentas)
                        .ThenInclude(dv => dv.Producto)
                    .FirstOrDefaultAsync(v => v.Id == id);
                if (venta == null)
                {
                    EscribirFichero.Escribir("No se encontró la Venta con el id: " + id);
                    return new VentaConDetallesDTO();
                }
                return new VentaConDetallesDTO
                {
                    Id = venta.Id,
                    FechaVenta = venta.FechaVenta,
                    Total = venta.Total,
                    NombreCliente = venta.Cliente.Nombre,
                    NombreUsuario = venta.Usuario.Nombre,
                    DetalleVentas = venta.DetalleVentas.Select(dv => new DetalleVentaDTO
                    {
                        Id = dv.Id,
                        NombreProducto = dv.Producto.Nombre,
                        Unidades = dv.Cantidad,
                        PrecioUnitario = dv.PrecioUnitario,
                        SubTotal = dv.Total
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                EscribirFichero.Escribir("Error al obtener la Venta por id: " + ex.Message);
                throw;
            }
        }

        // Obtener todas las Ventas
        public async Task<List<VentaConDetallesDTO>> GetAllVentasDTO()
        {
            try
            {
                return await _context.Ventas
                    .Include(v => v.Cliente)
                    .Include(v => v.Usuario)
                    .Select(v => new VentaConDetallesDTO
                    {
                        Id = v.Id,
                        FechaVenta = v.FechaVenta,
                        Total = v.Total,
                        NombreCliente = v.Cliente.Nombre,
                        NombreUsuario = v.Usuario.Nombre
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                EscribirFichero.Escribir("Error al obtener todas las Ventas: " + ex.Message);
                throw;
            }
        }

        // Crear Venta y sus detalles
        public async Task<Venta> InsertarVentaConDetalle(VentaConDetallesDTO dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var venta = new Venta
                {
                    FechaVenta = dto.FechaVenta,
                    Total = dto.Total,
                    ClienteId = _context.Clientes.FirstOrDefault(v => v.Nombre == dto.NombreCliente)?.Id,
                    UsuarioId = _context.Usuarios.FirstOrDefault(v => v.Nombre == dto.NombreUsuario)?.Id ?? 0,
                    DetalleVentas = new List<DetalleVenta>()
                };

                _context.Ventas.Add(venta);
                await _context.SaveChangesAsync();

                var movimientosStock = new List<MovimientoStock>();

                foreach (var detalle in dto.DetalleVentas)
                {
                    var detalleVenta = new DetalleVenta
                    {
                        VentaId = venta.Id,
                        ProductoId = _context.Productos.FirstOrDefault(p => p.Nombre == detalle.NombreProducto)?.Id,
                        Cantidad = detalle.Unidades,
                        PrecioUnitario = detalle.PrecioUnitario,
                        Total = detalle.SubTotal
                    };

                    venta.DetalleVentas.Add(detalleVenta);

                    var producto = _context.Productos.FirstOrDefault(p => p.Id == detalleVenta.ProductoId);
                    producto.Stock -= detalleVenta.Cantidad;

                    // Agregar movimiento de stock
                    movimientosStock.Add(new MovimientoStock
                    {
                        ProductoId = (int)detalleVenta.ProductoId,
                        Cantidad = detalleVenta.Cantidad,
                        TipoMovimiento = Movimiento.Salida,
                        FechaMovimiento = DateTime.Now,
                        VentaId = venta.Id,
                        UsuarioId = venta.UsuarioId
                    });
                }

                venta.CalcularTotal();

                _context.DetalleVentas.AddRange(venta.DetalleVentas);
                _context.MovimientoStocks.AddRange(movimientosStock);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return venta;
            }
            catch (Exception ex)
            {
                EscribirFichero.Escribir("Error al insertar la Venta: " + ex.Message);
                await transaction.RollbackAsync();
                throw;
            }
        }

        // Actualizar Venta
        public async Task ActualizarVenta(VentaConDetallesDTO dto)
        {
            try
            {
                var venta = await _context.Ventas
                    .Include(v => v.DetalleVentas)
                    .FirstOrDefaultAsync(v => v.Id == dto.Id);

                if (venta == null)
                {
                    EscribirFichero.Escribir("No se encontró la Venta con el id: " + dto.Id);
                    return;
                }

                // Actualizar los detalles de la venta
                venta.FechaVenta = dto.FechaVenta;
                venta.Total = dto.Total;
                venta.ClienteId = _context.Clientes.FirstOrDefault(v => v.Nombre == dto.NombreCliente)?.Id;
                venta.UsuarioId = _context.Usuarios.FirstOrDefault(v => v.Nombre == dto.NombreUsuario)?.Id ?? 0;

                // Recuperar y revertir los movimientos de stock
                var movimientos = _context.MovimientoStocks
                    .Where(m => m.VentaId == venta.Id)
                    .ToList();
                foreach (var movimiento in movimientos)
                {
                    var producto = _context.Productos.FirstOrDefault(p => p.Id == movimiento.ProductoId);
                    if (producto != null)
                    {
                        producto.Stock -= movimiento.Cantidad;
                        _context.Productos.Update(producto);
                    }
                }
                // Eliminar los movimientos de stock asociados a la venta
                _context.MovimientoStocks.RemoveRange(movimientos);

                // Eliminar los detalles existentes
                _context.DetalleVentas.RemoveRange(venta.DetalleVentas);

                var movimientoStock = new List<MovimientoStock>();

                // Agregar los nuevos detalles
                foreach (var detalle in dto.DetalleVentas)
                {
                    var detalleVenta = new DetalleVenta
                    {
                        ProductoId = _context.Productos.FirstOrDefault(p => p.Nombre == detalle.NombreProducto)?.Id,
                        Cantidad = detalle.Unidades,
                        PrecioUnitario = detalle.PrecioUnitario,
                        Total = detalle.SubTotal
                    };

                    // Agregar movimiento de stock
                    movimientoStock.Add(new MovimientoStock
                    {
                        ProductoId = (int)detalleVenta.ProductoId,
                        Cantidad = detalleVenta.Cantidad,
                        TipoMovimiento = Movimiento.Salida,
                        FechaMovimiento = DateTime.Now,
                        VentaId = venta.Id,
                        UsuarioId = venta.UsuarioId
                    });
                }

                venta.CalcularTotal();

                _context.Ventas.Update(venta);
                _context.DetalleVentas.AddRange(venta.DetalleVentas);
                _context.MovimientoStocks.AddRange(movimientoStock);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                EscribirFichero.Escribir("Error al actualizar la Venta: " + ex.Message);
                throw;
            }
        }
    }
}
