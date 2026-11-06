
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TallerMecanicoAPI.Models;

public partial class Cliente
{
    public int IdCliente { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(60)]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "El apellido es obligatorio")]
    [StringLength(60)]
    public string Apellido { get; set; } = null!;

    [Required(ErrorMessage = "El teléfono es obligatorio")]
    [StringLength(15, MinimumLength = 9)]
    public string Telefono { get; set; } = null!;

    [Required(ErrorMessage = "El correo es obligatorio")]
    [StringLength(100)]
    [EmailAddress(ErrorMessage = "El correo no es válido")]
    public string Correo { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Vehiculo> Vehiculos { get; set; } = new List<Vehiculo>();
}
