using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MidProject.Data;
using MidProject.Models;

namespace MidProject.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class ChargersController : ControllerBase
    {
        private readonly MidprojectDbContext _context;

        public ChargersController(MidprojectDbContext context)
        {
            _context = context;
        }

        // GET: api/Chargers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Charger>>> GetChargers()
        {
          if (_context.Chargers == null)
          {
              return NotFound();
          }
            return await _context.Chargers.ToListAsync();
        }

        // GET: api/Chargers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Charger>> GetCharger(int id)
        {
          if (_context.Chargers == null)
          {
              return NotFound();
          }
            var charger = await _context.Chargers.FindAsync(id);

            if (charger == null)
            {
                return NotFound();
            }

            return charger;
        }

        // PUT: api/Chargers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCharger(int id, Charger charger)
        {
            if (id != charger.ChargerId)
            {
                return BadRequest();
            }

            _context.Entry(charger).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChargerExists(id))
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

        // POST: api/Chargers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Charger>> PostCharger(Charger charger)
        {
          if (_context.Chargers == null)
          {
              return Problem("Entity set 'MidprojectDbContext.Chargers'  is null.");
          }
            _context.Chargers.Add(charger);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCharger", new { id = charger.ChargerId }, charger);
        }

        // DELETE: api/Chargers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCharger(int id)
        {
            if (_context.Chargers == null)
            {
                return NotFound();
            }
            var charger = await _context.Chargers.FindAsync(id);
            if (charger == null)
            {
                return NotFound();
            }

            _context.Chargers.Remove(charger);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ChargerExists(int id)
        {
            return (_context.Chargers?.Any(e => e.ChargerId == id)).GetValueOrDefault();
        }
    }
}
