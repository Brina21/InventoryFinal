namespace InventoryFinal.DTO
{
    public class CrearCompraDTO
    {
        public DateTime FechaCompra { get; set; } = DateTime.Now;
        public int? ClienteId { get; set; }
        public int UsuarioId { get; set; }

        public List<CrearDetalleCompraDTO>? DetalleCompras { get; set; } = new List<CrearDetalleCompraDTO>();
    }

    public class CrearDetalleCompraDTO
    {
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; } = 0;
        public int? ProductoId { get; set; }
    }
}
