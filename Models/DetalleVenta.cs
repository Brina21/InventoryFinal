using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryFinal.Models
{
    [Table("DetalleVenta")]
    public class DetalleVenta
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0.")]
        [DataType(DataType.Currency)]
        public decimal PrecioUnitario { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0.")]
        [DataType(DataType.Currency)]
        public decimal Total { get; set; }

        // Relaciones //
        public int VentaId { get; set; }
        public Venta? Venta { get; set; }

        public int ProductoId { get; set; }
        public Producto? Producto { get; set; }
    }
}
