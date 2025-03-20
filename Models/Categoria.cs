using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryFinal.Models
{
    [Table("Categoria")]
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Nombre { get; set; }

        public string? Descripcion { get; set; }

        // Relaciones //
        public List<Producto>? Productos { get; set; }
    }
}
