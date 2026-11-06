using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TallerMecanicoAPI.Models;

[Table("clientes")]
public partial class Cliente
{
    [Key]
    [Column("IdCliente")]
    public int IdCliente { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(60)]
    [Column("Nombre")]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "El apellido es obligatorio")]
    [StringLength(60)]
    [Column("Apellido")]
    public string Apellido { get; set; } = null!;

    [Required(ErrorMessage = "El teléfono es obligatorio")]
    [StringLength(15, MinimumLength = 9)]
    [Column("Telefono")]
    public string Telefono { get; set; } = null!;

    [Required(ErrorMessage = "El correo es obligatorio")]
    [StringLength(100)]
    [EmailAddress(ErrorMessage = "El correo no es válido")]
    [Column("Correo")]
    public string Correo { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Vehiculo> Vehiculos { get; set; } = new List<Vehiculo>();
}