
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TallerMecanicoAPI.Data;
using TallerMecanicoAPI.Models;

namespace TallerMecanicoAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClientesController : ControllerBase
{
    private readonly TallerMecanicoContext _context;

    public ClientesController(TallerMecanicoContext context)
    {
        _context = context;
    }

    // GET: api/clientes
    // GET: api/clientes?nombre=Juan&correo=gmail
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes(
        string? nombre, string? correo)
    {
        var query = _context.Clientes.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(nombre))
            query = query.Where(c => c.Nombre.Contains(nombre));

        if (!string.IsNullOrWhiteSpace(correo))
            query = query.Where(c => c.Correo.Contains(correo));

        return Ok(await query.OrderBy(c => c.IdCliente).ToListAsync());
    }

    // GET: api/clientes/1
    [HttpGet("{id}")]
    public async Task<ActionResult<Cliente>> GetCliente(int id)
    {
        var cliente = await _context.Clientes
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.IdCliente == id);

        if (cliente == null)
            return NotFound();

        return Ok(cliente);
    }

    // GET: api/clientes/1/vehiculos
    [HttpGet("{id}/vehiculos")]
    public async Task<ActionResult<IEnumerable<Vehiculo>>> GetVehiculosCliente(int id)
    {
        if (!await _context.Clientes.AnyAsync(c => c.IdCliente == id))
            return NotFound();

        var vehiculos = await _context.Vehiculos
            .AsNoTracking()
            .Where(v => v.IdCliente == id)
            .Include(v => v.IdClienteNavigation)
            .ToListAsync();

        return Ok(vehiculos);
    }

    // POST: api/clientes
    [HttpPost]
    public async Task<ActionResult<Cliente>> PostCliente(Cliente cliente)
    {
        bool existe = await _context.Clientes
            .AnyAsync(c => c.Correo == cliente.Correo);

        if (existe)
            return Conflict("Ya existe un cliente con ese correo.");

        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetCliente),
            new { id = cliente.IdCliente },
            cliente
        );
    }

    // PUT: api/clientes/1
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCliente(int id, Cliente cliente)
    {
        if (id != cliente.IdCliente)
            return BadRequest("El ID de la ruta no coincide con el cuerpo.");

        var existente = await _context.Clientes.FindAsync(id);

        if (existente == null)
            return NotFound();

        bool correoDuplicado = await _context.Clientes
            .AnyAsync(c => c.Correo == cliente.Correo && c.IdCliente != id);

        if (correoDuplicado)
            return Conflict("El correo ya está registrado por otro cliente.");

        existente.Nombre = cliente.Nombre;
        existente.Apellido = cliente.Apellido;
        existente.Telefono = cliente.Telefono;
        existente.Correo = cliente.Correo;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/clientes/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCliente(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);

        if (cliente == null)
            return NotFound();

        bool tieneVehiculos = await _context.Vehiculos
            .AnyAsync(v => v.IdCliente == id);

        if (tieneVehiculos)
            return Conflict("No se puede eliminar el cliente porque tiene vehículos registrados.");

        _context.Clientes.Remove(cliente);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
