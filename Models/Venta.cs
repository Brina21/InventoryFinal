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
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0.")]
        [DataType(DataType.Currency)]
        public decimal Total { get; set; }

        public void CalcularTotal()
        {
            if (DetalleVentas != null)
            {
                Total = DetalleVentas.Sum(dc => dc.Total);
            }
            else
            {
                Total = 0;
            }
        }

        // Relaciones //
        public int? ClienteId { get; set; }
        public Cliente? Cliente { get; set; }

        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        public List<DetalleVenta>? DetalleVentas { get; set; }

        public List<MovimientoStock>? MovimientoStocks { get; set; }
    }
}
