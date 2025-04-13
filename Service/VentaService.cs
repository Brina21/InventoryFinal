using InventoryFinal.EscribirLogsFichero;
using InventoryFinal.Models;
using InventoryFinal.Repository;
using Microsoft.IdentityModel.Tokens;

namespace InventoryFinal.Service
{
    public class VentaService
    {
        private readonly VentaRepository ventaRepository;

        public VentaService(VentaRepository ventaRepository)
        {
            this.ventaRepository = ventaRepository;
        }

        public async Task<(bool exito, string mensaje, Venta? venta)> ObtenerVentaPorId(int id)
        {
            var entidad = await ventaRepository.GetByVentaId(id);

            if (entidad == null)
            {
                EscribirFichero.Escribir($"No se ha podido obtener la Venta con id{entidad?.Id}");
                return (false, $"No se ha podido obtener la Venta con id{entidad?.Id}", null);
            }

            return (true, $"Se ha obtenido la Venta con id{entidad?.Id}", entidad);
        }

        public async Task<(bool exito, string mensaje, List<Venta> ventas)> ObtenerTodasVentas()
        {
            List<Venta> resultado = await ventaRepository.GetAllVentas();

            if (!resultado.IsNullOrEmpty())
            {
                return (true, "Ventas obtenidas correctamente", resultado);
            }

            EscribirFichero.Escribir("No se han obtenido las ventas");
            return (false, "No se encontraron ventas", new List<Venta>());
        }
    }
}
