using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoFinal.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatoesController : ControllerBase
    {
        private readonly RestauranteContext _context;

        public PlatoesController(RestauranteContext context)
        {
            _context = context;
        }

        // GET: api/Platoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Plato>>> GetPlatos(string tipo = null, string nombre = null, string ordenar = null)
        {
            var platos = _context.Platos.AsQueryable();

            // Filtrar por tipo
            if (!string.IsNullOrEmpty(tipo))
            {
                platos = platos.Where(p => p.Tipo == tipo);
            }

            // Buscar por nombre
            if (!string.IsNullOrEmpty(nombre))
            {
                platos = platos.Where(p => p.Nombre.Contains(nombre));
            }

            // Ordenar por precio
            if (!string.IsNullOrEmpty(ordenar) && ordenar.ToLower() == "precio")
            {
                platos = platos.OrderBy(p => p.Precio);
            }

            var resultado = await platos.ToListAsync();

            if (resultado == null || !resultado.Any())
            {
                return NotFound("No se encontraron platos con los criterios especificados.");
            }

            return resultado;
        }

        // GET: api/Platoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Plato>> GetPlato(int id)
        {
            var plato = await _context.Platos.FindAsync(id);

            if (plato == null)
            {
                return NotFound($"No se encontró un plato con el ID {id}.");
            }

            return plato;
        }

        // POST: api/Platoes
        [HttpPost]
        public async Task<ActionResult<Plato>> CreatePlato(Plato plato)
        {
            _context.Platos.Add(plato);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPlato), new { id = plato.Id }, plato);
        }

        // PUT: api/Platoes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlato(int id, Plato plato)
        {
            if (id != plato.Id)
            {
                return BadRequest("El ID del plato no coincide.");
            }

            _context.Entry(plato).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlatoExists(id))
                {
                    return NotFound($"No se encontró un plato con el ID {id}.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Platoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlato(int id)
        {
            var plato = await _context.Platos.FindAsync(id);
            if (plato == null)
            {
                return NotFound($"No se encontró un plato con el ID {id}.");
            }

            _context.Platos.Remove(plato);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PlatoExists(int id)
        {
            return _context.Platos.Any(e => e.Id == id);
        }
    }
}