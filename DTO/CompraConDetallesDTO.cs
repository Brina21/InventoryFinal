namespace InventoryFinal.DTO
{
    // DTO para mostrar DetallesCompra junto con la Compra
    public class CompraConDetallesDTO
    {
        public int Id { get; set; }
        public DateTime FechaCompra { get; set; } = DateTime.Now;
        public decimal Total { get; set; }
        public string? NombreUsuario { get; set; }
        public string? NombreCliente { get; set; }

        public List<DetalleCompraDTO> DetalleCompras { get; set; } = new List<DetalleCompraDTO>();
    }

    public class DetalleCompraDTO
    {
        public int Id { get; set; }
        public string? NombreProducto { get; set; }
        public int Unidades { get; set; }
        public decimal PrecioUnitario { get; set; } = 0;
        public decimal SubTotal { get; set; } = 0;

    }
}
