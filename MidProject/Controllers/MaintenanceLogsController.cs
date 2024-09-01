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
    public class MaintenanceLogsController : ControllerBase
    {
        private readonly MidprojectDbContext _context;

        public MaintenanceLogsController(MidprojectDbContext context)
        {
            _context = context;
        }

        // GET: api/MaintenanceLogs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MaintenanceLog>>> GetMaintenanceLogs()
        {
          if (_context.MaintenanceLogs == null)
          {
              return NotFound();
          }
            return await _context.MaintenanceLogs.ToListAsync();
        }

        // GET: api/MaintenanceLogs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MaintenanceLog>> GetMaintenanceLog(int id)
        {
          if (_context.MaintenanceLogs == null)
          {
              return NotFound();
          }
            var maintenanceLog = await _context.MaintenanceLogs.FindAsync(id);

            if (maintenanceLog == null)
            {
                return NotFound();
            }

            return maintenanceLog;
        }

        // PUT: api/MaintenanceLogs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMaintenanceLog(int id, MaintenanceLog maintenanceLog)
        {
            if (id != maintenanceLog.MaintenanceLogId)
            {
                return BadRequest();
            }

            _context.Entry(maintenanceLog).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MaintenanceLogExists(id))
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

        // POST: api/MaintenanceLogs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MaintenanceLog>> PostMaintenanceLog(MaintenanceLog maintenanceLog)
        {
          if (_context.MaintenanceLogs == null)
          {
              return Problem("Entity set 'MidprojectDbContext.MaintenanceLogs'  is null.");
          }
            _context.MaintenanceLogs.Add(maintenanceLog);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMaintenanceLog", new { id = maintenanceLog.MaintenanceLogId }, maintenanceLog);
        }

        // DELETE: api/MaintenanceLogs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaintenanceLog(int id)
        {
            if (_context.MaintenanceLogs == null)
            {
                return NotFound();
            }
            var maintenanceLog = await _context.MaintenanceLogs.FindAsync(id);
            if (maintenanceLog == null)
            {
                return NotFound();
            }

            _context.MaintenanceLogs.Remove(maintenanceLog);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MaintenanceLogExists(int id)
        {
            return (_context.MaintenanceLogs?.Any(e => e.MaintenanceLogId == id)).GetValueOrDefault();
        }
    }
}
