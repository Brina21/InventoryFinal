using InventoryFinal.EscribirLogsFichero;
using InventoryFinal.Models;
using InventoryFinal.Repository;
using Microsoft.IdentityModel.Tokens;

namespace InventoryFinal.Service
{
    public class CompraService : GenericoService<Compra>
    {
        private readonly CompraRepository compraRepository;

        public CompraService(CompraRepository compraRepository) :base(compraRepository)
        {
            this.compraRepository = compraRepository;
        }

        public async Task<(bool exito, string mensaje, Compra? compra)> ObtenerCompraPorId(int id)
        {
            var entidad = await compraRepository.GetCompraId(id);

            if (entidad == null)
            {
                EscribirFichero.Escribir("No se ha podido obtener la Compra.");
                return (false, "No se ha podido obtener la Compra", null);
            }

            return (true, "Se ha obtenido la Compra", entidad);
        }

        public async Task<(bool exito, string mensaje, List<Compra> compras)> ObtenerTodasCompras()
        {
            List<Compra> _compras = await compraRepository.GetAllCompra();

            if(_compras.IsNullOrEmpty())
            {
                EscribirFichero.Escribir("No se han obtenido las Compras");
                return (false, "Compras Obtenidas correctamente", new List<Compra>());
            }

            return (true, "Se han obtenido las Compras",  _compras);
        }
    }
}
