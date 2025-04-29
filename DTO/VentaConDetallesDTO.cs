namespace InventoryFinal.DTO
{
    // DTO para mostrar DetallesVenta junto con la Venta
    public class VentaConDetallesDTO
    {
        public int Id { get; set; }
        public DateTime FechaVenta { get; set; } = DateTime.Now;
        public decimal Total { get; set; }
        public string? NombreUsuario { get; set; }
        public string? NombreCliente { get; set; }

        public List<DetalleVentaDTO> DetalleVentas { get; set; } = new List<DetalleVentaDTO>();
    }

    public class DetalleVentaDTO
    {
        public int Id { get; set; }
        public string? NombreProducto { get; set; }
        public int Unidades { get; set; }
        public decimal PrecioUnitario { get; set; } = 0;
        public decimal SubTotal { get; set; } = 0;

    }
}
