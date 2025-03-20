using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryFinal.Models
{
    [Table("Cliente")]
    public class Cliente
    {
        [Key]
        public int Id { get; set; }
        
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [StringLength(255)]
        public required string Nombre { get; set; }

        public int Telefono { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        // Relaciones //
        public List<Venta>? Ventas { get; set; }

        public List<Compra>? Compras { get; set; }
    }
}
