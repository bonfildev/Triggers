using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TriggersAPI.Data;
using TriggersModels;

namespace TriggersAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgendaController : ControllerBase
    {
        private readonly AppDBContext _context;

        public AgendaController(AppDBContext context)
        {
            _context = context;
        }

        // GET: api/Agenda
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Agenda>>> GetAgendas()
        {
            return await _context.Agendas.ToListAsync();
        }

        // GET: api/Agenda/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Agenda>> GetAgenda(string id)
        {
            var agenda = await _context.Agendas.FindAsync(id);

            if (agenda == null)
            {
                return NotFound();
            }

            return agenda;
        }

        // PUT: api/Agenda/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAgenda(string id, Agenda agenda)
        {
            if (id != agenda.Id)
            {
                return BadRequest();
            }

            _context.Entry(agenda).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AgendaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Agenda
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Agenda>> PostAgenda(Agenda agenda)
        {
            agenda.Id = Guid.NewGuid().ToString();
            _context.Agendas.Add(agenda);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AgendaExists(agenda.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetAgenda", new { id = agenda.Id }, agenda);
        }

        // DELETE: api/Agenda/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAgenda(string id)
        {
            var agenda = await _context.Agendas.FindAsync(id);
            if (agenda == null)
            {
                return NotFound();
            }

            _context.Agendas.Remove(agenda);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AgendaExists(string id)
        {
            return _context.Agendas.Any(e => e.Id == id);
        }
    }
}
