
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TallerMecanicoAPI.Data;
using TallerMecanicoAPI.Models;

namespace TallerMecanicoAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdenesServicioController : ControllerBase
{
    private readonly TallerMecanicoContext _context;

    public OrdenesServicioController(TallerMecanicoContext context)
    {
        _context = context;
    }

    // GET: api/ordenesservicio
    // GET: api/ordenesservicio?estado=Pendiente&idVehiculo=1
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrdenesServicio>>> GetOrdenes(
        string? estado, int? idVehiculo,
        DateTime? fechaDesde, DateTime? fechaHasta)
    {
        var query = _context.OrdenesServicios
            .AsNoTracking()
            .Include(o => o.IdVehiculoNavigation)
                .ThenInclude(v => v!.IdClienteNavigation)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(estado))
            query = query.Where(o => o.Estado.Contains(estado));

        if (idVehiculo.HasValue)
            query = query.Where(o => o.IdVehiculo == idVehiculo.Value);

        if (fechaDesde.HasValue)
            query = query.Where(o => o.Fecha >= fechaDesde.Value);

        if (fechaHasta.HasValue)
            query = query.Where(o => o.Fecha <= fechaHasta.Value);

        return Ok(await query.OrderByDescending(o => o.Fecha).ToListAsync());
    }

    // GET: api/ordenesservicio/1
    [HttpGet("{id}")]
    public async Task<ActionResult<OrdenesServicio>> GetOrden(int id)
    {
        var orden = await _context.OrdenesServicios
            .AsNoTracking()
            .Include(o => o.IdVehiculoNavigation)
            .FirstOrDefaultAsync(o => o.IdOrden == id);

        if (orden == null)
            return NotFound();

        return Ok(orden);
    }

    // POST: api/ordenesservicio
    [HttpPost]
    public async Task<ActionResult<OrdenesServicio>> PostOrden(OrdenesServicio orden)
    {
        bool vehiculoExiste = await _context.Vehiculos
            .AnyAsync(v => v.IdVehiculo == orden.IdVehiculo);

        if (!vehiculoExiste)
            return BadRequest("El vehículo indicado no existe.");

        orden.IdVehiculoNavigation = null;

        _context.OrdenesServicios.Add(orden);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetOrden),
            new { id = orden.IdOrden },
            orden
        );
    }

    // PUT: api/ordenesservicio/1
    [HttpPut("{id}")]
    public async Task<IActionResult> PutOrden(int id, OrdenesServicio orden)
    {
        if (id != orden.IdOrden)
            return BadRequest("El ID de la ruta no coincide con el cuerpo.");

        var existente = await _context.OrdenesServicios.FindAsync(id);

        if (existente == null)
            return NotFound();

        bool vehiculoExiste = await _context.Vehiculos
            .AnyAsync(v => v.IdVehiculo == orden.IdVehiculo);

        if (!vehiculoExiste)
            return BadRequest("El vehículo indicado no existe.");

        existente.Fecha = orden.Fecha;
        existente.Descripcion = orden.Descripcion;
        existente.Estado = orden.Estado;
        existente.Costo = orden.Costo;
        existente.IdVehiculo = orden.IdVehiculo;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/ordenesservicio/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrden(int id)
    {
        var orden = await _context.OrdenesServicios.FindAsync(id);

        if (orden == null)
            return NotFound();

        _context.OrdenesServicios.Remove(orden);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
