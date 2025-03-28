using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InventoryFinal.Models
{
    [Table("Compra")]
    public class Compra
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required DateTime FechaCompra { get; set; } = DateTime.Now;

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Total { get; set; }

        // Relaciones //
        public int? ClienteId { get; set; }
        public Cliente? Cliente { get; set; }

        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        public List<MovimientoStock>? MovimientoStocks { get; set; }

        public List<DetalleCompra>? DetalleCompras { get; set; }
    }
}
