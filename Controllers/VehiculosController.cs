
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TallerMecanicoAPI.Data;
using TallerMecanicoAPI.Models;

namespace TallerMecanicoAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VehiculosController : ControllerBase
{
    private readonly TallerMecanicoContext _context;

    public VehiculosController(TallerMecanicoContext context)
    {
        _context = context;
    }

    // GET: api/vehiculos
    // GET: api/vehiculos?marca=Toyota&idCliente=1
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Vehiculo>>> GetVehiculos(
        string? marca, string? placa, int? idCliente)
    {
        var query = _context.Vehiculos
            .AsNoTracking()
            .Include(v => v.IdClienteNavigation)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(marca))
            query = query.Where(v => v.Marca.Contains(marca));

        if (!string.IsNullOrWhiteSpace(placa))
            query = query.Where(v => v.Placa.Contains(placa));

        if (idCliente.HasValue)
            query = query.Where(v => v.IdCliente == idCliente.Value);

        return Ok(await query.OrderBy(v => v.IdVehiculo).ToListAsync());
    }

    // GET: api/vehiculos/1
    [HttpGet("{id}")]
    public async Task<ActionResult<Vehiculo>> GetVehiculo(int id)
    {
        var vehiculo = await _context.Vehiculos
            .AsNoTracking()
            .Include(v => v.IdClienteNavigation)
            .FirstOrDefaultAsync(v => v.IdVehiculo == id);

        if (vehiculo == null)
            return NotFound();

        return Ok(vehiculo);
    }

    // GET: api/vehiculos/1/ordenes
    [HttpGet("{id}/ordenes")]
    public async Task<ActionResult<IEnumerable<OrdenesServicio>>> GetOrdenesVehiculo(int id)
    {
        if (!await _context.Vehiculos.AnyAsync(v => v.IdVehiculo == id))
            return NotFound();

        var ordenes = await _context.OrdenesServicios
            .AsNoTracking()
            .Where(o => o.IdVehiculo == id)
            .Include(o => o.IdVehiculoNavigation)
            .ToListAsync();

        return Ok(ordenes);
    }

    // POST: api/vehiculos
    [HttpPost]
    public async Task<ActionResult<Vehiculo>> PostVehiculo(Vehiculo vehiculo)
    {
        bool clienteExiste = await _context.Clientes
            .AnyAsync(c => c.IdCliente == vehiculo.IdCliente);

        if (!clienteExiste)
            return BadRequest("El cliente indicado no existe.");

        bool placaExiste = await _context.Vehiculos
            .AnyAsync(v => v.Placa == vehiculo.Placa);

        if (placaExiste)
            return Conflict("La placa ya está registrada.");

        vehiculo.IdClienteNavigation = null;
        vehiculo.OrdenesServicios = new List<OrdenesServicio>();

        _context.Vehiculos.Add(vehiculo);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetVehiculo),
            new { id = vehiculo.IdVehiculo },
            vehiculo
        );
    }

    // PUT: api/vehiculos/1
    [HttpPut("{id}")]
    public async Task<IActionResult> PutVehiculo(int id, Vehiculo vehiculo)
    {
        if (id != vehiculo.IdVehiculo)
            return BadRequest("El ID de la ruta no coincide con el cuerpo.");

        var existente = await _context.Vehiculos.FindAsync(id);

        if (existente == null)
            return NotFound();

        bool clienteExiste = await _context.Clientes
            .AnyAsync(c => c.IdCliente == vehiculo.IdCliente);

        if (!clienteExiste)
            return BadRequest("El cliente indicado no existe.");

        bool placaDuplicada = await _context.Vehiculos
            .AnyAsync(v => v.Placa == vehiculo.Placa && v.IdVehiculo != id);

        if (placaDuplicada)
            return Conflict("La placa ya está registrada por otro vehículo.");

        existente.Placa = vehiculo.Placa;
        existente.Marca = vehiculo.Marca;
        existente.Modelo = vehiculo.Modelo;
        existente.Anio = vehiculo.Anio;
        existente.IdCliente = vehiculo.IdCliente;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/vehiculos/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVehiculo(int id)
    {
        var vehiculo = await _context.Vehiculos.FindAsync(id);

        if (vehiculo == null)
            return NotFound();

        bool tieneOrdenes = await _context.OrdenesServicios
            .AnyAsync(o => o.IdVehiculo == id);

        if (tieneOrdenes)
            return Conflict("No se puede eliminar el vehículo porque tiene órdenes de servicio.");

        _context.Vehiculos.Remove(vehiculo);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
