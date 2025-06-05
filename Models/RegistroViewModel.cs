using InventoryFinal.Models;
using System.ComponentModel.DataAnnotations;

public class RegisterViewModel
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(255)]
    public string Nombre { get; set; }

    [Required(ErrorMessage = "El correo es obligatorio")]
    [EmailAddress(ErrorMessage = "Correo no válido")]
    public string Email { get; set; }

    [Required(ErrorMessage = "La contraseña es obligatoria")]
    [StringLength(255, MinimumLength = 6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
    [DataType(DataType.Password)]
    public string Contrasenya { get; set; }

    [Required(ErrorMessage = "Confirma tu contraseña")]
    [Compare("Contrasenya", ErrorMessage = "Las contraseñas no coinciden")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; }

    // Opcional: Teléfono y Rol si quieres que se registren
    public int Telefono { get; set; } = 0;

    public Cargo Rol { get; set; } = Cargo.Administrador; // Por defecto
}
