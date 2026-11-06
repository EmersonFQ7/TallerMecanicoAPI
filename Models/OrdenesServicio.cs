
using System.ComponentModel.DataAnnotations;

namespace TallerMecanicoAPI.Models;

public partial class OrdenesServicio
{
    public int IdOrden { get; set; }

    [Required(ErrorMessage = "La fecha es obligatoria")]
    public DateTime Fecha { get; set; }

    [Required(ErrorMessage = "La descripción es obligatoria")]
    [StringLength(200)]
    public string Descripcion { get; set; } = null!;

    [Required(ErrorMessage = "El estado es obligatorio")]
    [StringLength(30)]
    public string Estado { get; set; } = null!;

    [Range(typeof(decimal), "0.01", "99999999.99",
        ErrorMessage = "El costo debe ser mayor que cero")]
    public decimal Costo { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un vehículo válido")]
    public int IdVehiculo { get; set; }

    public virtual Vehiculo? IdVehiculoNavigation { get; set; }
}
