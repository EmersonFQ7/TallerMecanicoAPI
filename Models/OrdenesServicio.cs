using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TallerMecanicoAPI.Models;

[Table("ordenesservicio")]
public partial class OrdenesServicio
{
    [Key]
    [Column("IdOrden")]
    public int IdOrden { get; set; }

    [Required(ErrorMessage = "La fecha es obligatoria")]
    [Column("Fecha")]
    public DateTime Fecha { get; set; }

    [Required(ErrorMessage = "La descripción es obligatoria")]
    [StringLength(200)]
    [Column("Descripcion")]
    public string Descripcion { get; set; } = null!;

    [Required(ErrorMessage = "El estado es obligatorio")]
    [StringLength(30)]
    [Column("Estado")]
    public string Estado { get; set; } = null!;

    [Range(typeof(decimal), "0.01", "99999999.99",
        ErrorMessage = "El costo debe ser mayor que cero")]
    [Column("Costo")]
    public decimal Costo { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un vehículo válido")]
    [Column("IdVehiculo")]
    public int IdVehiculo { get; set; }

    [ForeignKey("IdVehiculo")]
    public virtual Vehiculo? IdVehiculoNavigation { get; set; }
}