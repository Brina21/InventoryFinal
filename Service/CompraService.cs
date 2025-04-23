using InventoryFinal.Data;
using InventoryFinal.DTO;
using InventoryFinal.EscribirLogsFichero;
using InventoryFinal.Models;
using InventoryFinal.Repository;
using Microsoft.EntityFrameworkCore;

namespace InventoryFinal.Service
{
    public class CompraService
    {
        private readonly ApplicationDbContext _context;
        private readonly CompraRepository _compraRepository;

        public CompraService(CompraRepository compraRepository)
        {
            _compraRepository = compraRepository;
        }

        public async Task<List<CompraConDetallesDTO>> ObtenerTodasLasComprasDTO()
        {
            return await _compraRepository.GetAllComprasDTO();
        }

        public async Task<CompraConDetallesDTO?> ObtenerCompraConDetallesPorId(int id)
        {
            return await _compraRepository.GetCompraDTOById(id);
        }

        public async Task<(bool exito, string mensaje, Compra nuevaCompra)> CrearCompraDTO(CompraConDetallesDTO dto)
        {
            try
            {
                var nuevaCompra = await _compraRepository.InsertarCompraConDetalle(dto);
                if (nuevaCompra == null)
                {
                    return (false, "No se pudo insertar la compra.", null);
                }

                return (true, "Compra registrada correctamente.", nuevaCompra);
            }
            catch (Exception ex)
            {
                EscribirFichero.Escribir($"Error al guardar la compra: {ex.Message}");
                return (false, $"Error al guardar la compra: {ex.Message}", null);
            }
        }



        public async Task ActualizarCompra(CompraConDetallesDTO dto)
        {
            await _compraRepository.ActualizarCompra(dto);
        }

        public async Task EliminarCompra(int id)
        {
            await _compraRepository.EliminarCompra(id);
        }
    }
}
