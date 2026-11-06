
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TallerMecanicoAPI.Models;

public partial class Vehiculo
{
    public int IdVehiculo { get; set; }

    [Required(ErrorMessage = "La placa es obligatoria")]
    [StringLength(10)]
    public string Placa { get; set; } = null!;

    [Required(ErrorMessage = "La marca es obligatoria")]
    [StringLength(50)]
    public string Marca { get; set; } = null!;

    [Required(ErrorMessage = "El modelo es obligatorio")]
    [StringLength(50)]
    public string Modelo { get; set; } = null!;

    [Range(1900, 2100, ErrorMessage = "El año no es válido")]
    public int Anio { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un cliente válido")]
    public int IdCliente { get; set; }

    public virtual Cliente? IdClienteNavigation { get; set; }

    [JsonIgnore]
    public virtual ICollection<OrdenesServicio> OrdenesServicios { get; set; } = new List<OrdenesServicio>();
}
