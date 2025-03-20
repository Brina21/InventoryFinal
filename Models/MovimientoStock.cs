using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryFinal.Models
{
    [Table("MovimientoStock")]
    public class MovimientoStock
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        public Movimiento TipoMovimiento { get; set; }

        [Required]
        public DateTime FechaMovimiento { get; set; } = DateTime.Now;

        // Relaciones //
        public int ProductoId { get; set; }
        public Producto? Producto { get; set; }

        public int VentaId { get; set; }
        public Venta? Venta { get; set; }

        public int CompraId { get; set; }
        public Compra? Compra { get; set; }

        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
    }

    public enum Movimiento
    {
        Entrada,
        Salida
    }
}
