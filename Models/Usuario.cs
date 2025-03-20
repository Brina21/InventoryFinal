using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryFinal.Models
{
    [Table("Usuario")]
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public required string Nombre { get; set; }

        public int Telefono { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public Cargo Rol { get; set; }

        [Required]
        [StringLength(255)]
        public required string Contrasenya { get; set; }

        // Campo oculto //
        [Required]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        // Relaciones //
        public List<Venta>? Ventas { get; set; }

        public List<Compra>? Compras { get; set; }

        public List<MovimientoStock>? MovimientoStock { get; set; }
    }

    public enum Cargo
    {
        Administrador,
        Empleado
    }
}
