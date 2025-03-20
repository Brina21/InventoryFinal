using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryFinal.Models
{
    [Table("Proveedor")]
    public class Proveedor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Nombre { get; set; }
        
        [Required]
        public required string Contacto { get; set; }
        
        [Required]
        public required int Telefono { get; set; }
        
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        // Relaciones //
        public List<Producto>? Productos { get; set; }
    }
}
