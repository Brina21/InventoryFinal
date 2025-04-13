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
        [Column(TypeName = "decimal(10,2)")]
        public decimal PrecioUnitario { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Total { get; set; }

        // Relaciones //
        public int CompraId { get; set; }
        public Compra? Compra { get; set; }

        public int? ProductoId { get; set; }
        public Producto? Producto { get; set; }
    }
}
