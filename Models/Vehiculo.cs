using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TallerMecanicoAPI.Models;

[Table("vehiculos")]
public partial class Vehiculo
{
    [Key]
    [Column("IdVehiculo")]
    public int IdVehiculo { get; set; }

    [Required(ErrorMessage = "La placa es obligatoria")]
    [StringLength(10)]
    [Column("Placa")]
    public string Placa { get; set; } = null!;

    [Required(ErrorMessage = "La marca es obligatoria")]
    [StringLength(50)]
    [Column("Marca")]
    public string Marca { get; set; } = null!;

    [Required(ErrorMessage = "El modelo es obligatorio")]
    [StringLength(50)]
    [Column("Modelo")]
    public string Modelo { get; set; } = null!;

    [Range(1900, 2100, ErrorMessage = "El año no es válido")]
    [Column("Anio")]
    public int Anio { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un cliente válido")]
    [Column("IdCliente")]
    public int IdCliente { get; set; }

    [ForeignKey("IdCliente")]
    public virtual Cliente? IdClienteNavigation { get; set; }

    [JsonIgnore]
    public virtual ICollection<OrdenesServicio> OrdenesServicios { get; set; } = new List<OrdenesServicio>();
}
