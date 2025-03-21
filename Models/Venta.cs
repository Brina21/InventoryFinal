using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryFinal.Models
{
    [Table("Venta")]
    public class Venta
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required DateTime FechaVenta { get; set; } = DateTime.Now;

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Total { get; set; }

        // Relaciones //
        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }

        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        public List<DetalleVenta>? DetalleVentas { get; set; }

        public List<MovimientoStock>? MovimientoStocks { get; set; }
    }
}
