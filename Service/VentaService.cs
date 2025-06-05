using InventoryFinal.DTO;
using InventoryFinal.EscribirLogsFichero;
using InventoryFinal.Models;
using InventoryFinal.Repository;

namespace InventoryFinal.Service
{
    public class VentaService
    {
        private readonly VentaRepository ventaRepository;

        public VentaService(VentaRepository ventaRepository)
        {
            this.ventaRepository = ventaRepository;
        }

        public async Task<List<VentaConDetallesDTO>> ObtenerTodasVentas()
        {
            return await ventaRepository.GetAllVentasDTO();
        }

        public async Task<VentaConDetallesDTO?> ObtenerVentaPorId(int id)
        {
            return await ventaRepository.GetVentaDTOById(id);
        }

        public async Task<(bool exito, string mensaje, Venta nuevaVenta)> CrearVentaDTO(VentaConDetallesDTO dto)
        {
            try
            {
                var nuevaVenta = await ventaRepository.InsertarVentaConDetalle(dto);
                if (nuevaVenta == null)
                {
                    return (false, "No se pudo insertar la venta.", null);
                }
                return (true, "Venta registrada correctamente.", nuevaVenta);
            }
            catch (Exception ex)
            {
                EscribirFichero.Escribir($"Error al guardar la venta: {ex.Message}");
                return (false, $"Error al guardar la venta: {ex.Message}", null);
            }
        }

        public async Task ActualizarVenta(VentaConDetallesDTO dto)
        {
            await ventaRepository.ActualizarVenta(dto);
        }
    }
}
