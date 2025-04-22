using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InventoryFinal.Models
{
    [Table("DetalleCompra")]
    public class DetalleCompra
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
        public decimal Total { get; private set; }

        public void CalcularTotal()
        {
            Total = Cantidad * PrecioUnitario;
        }

        // Relaciones //
        public int CompraId { get; set; }
        public Compra? Compra { get; set; }

        public int? ProductoId { get; set; }
        public Producto? Producto { get; set; }
    }
}
