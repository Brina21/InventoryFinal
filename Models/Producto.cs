using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryFinal.Models
{
    [Table("Producto")]
    public class Producto
    {
        [Key]
        public int Id { get; set; }

        public byte[]? Imagen { get; set; }

        [Required]
        [StringLength(255)]
        public required string Nombre { get; set; }

        public string? Descripcion { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0.")]
        [DataType(DataType.Currency)]
        public decimal Precio { get; set; }

        [Required]
        public int Stock { get; set; }

        [Required]
        public int StockMinimo { get; set; }

        // Relaciones //
        public int? CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }

        public int? ProveedorId { get; set; }
        public Proveedor? Proveedor { get; set; }

        public List<DetalleCompra>? DetalleCompras { get; set; }

        public List<DetalleVenta>? DetalleVentas { get; set; }

        public List<MovimientoStock>? MovimientoStocks { get; set; }
    }
}
