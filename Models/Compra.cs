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
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0.")]
        [DataType(DataType.Currency)]
        public decimal Total { get; set; }

        public void CalcularTotal()
        {
            if (DetalleCompras != null)
            {
                Total = DetalleCompras.Sum(dc => dc.Total);
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

        public List<MovimientoStock>? MovimientoStocks { get; set; }

        public List<DetalleCompra>? DetalleCompras { get; set; }
    }
}
