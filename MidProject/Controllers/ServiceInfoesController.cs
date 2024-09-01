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
    public class ServiceInfoesController : ControllerBase
    {
        private readonly MidprojectDbContext _context;

        public ServiceInfoesController(MidprojectDbContext context)
        {
            _context = context;
        }

        // GET: api/ServiceInfoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceInfo>>> GetServiceInfos()
        {
          if (_context.ServiceInfos == null)
          {
              return NotFound();
          }
            return await _context.ServiceInfos.ToListAsync();
        }

        // GET: api/ServiceInfoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceInfo>> GetServiceInfo(int id)
        {
          if (_context.ServiceInfos == null)
          {
              return NotFound();
          }
            var serviceInfo = await _context.ServiceInfos.FindAsync(id);

            if (serviceInfo == null)
            {
                return NotFound();
            }

            return serviceInfo;
        }

        // PUT: api/ServiceInfoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutServiceInfo(int id, ServiceInfo serviceInfo)
        {
            if (id != serviceInfo.ServiceInfoId)
            {
                return BadRequest();
            }

            _context.Entry(serviceInfo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceInfoExists(id))
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

        // POST: api/ServiceInfoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ServiceInfo>> PostServiceInfo(ServiceInfo serviceInfo)
        {
          if (_context.ServiceInfos == null)
          {
              return Problem("Entity set 'MidprojectDbContext.ServiceInfos'  is null.");
          }
            _context.ServiceInfos.Add(serviceInfo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetServiceInfo", new { id = serviceInfo.ServiceInfoId }, serviceInfo);
        }

        // DELETE: api/ServiceInfoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServiceInfo(int id)
        {
            if (_context.ServiceInfos == null)
            {
                return NotFound();
            }
            var serviceInfo = await _context.ServiceInfos.FindAsync(id);
            if (serviceInfo == null)
            {
                return NotFound();
            }

            _context.ServiceInfos.Remove(serviceInfo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ServiceInfoExists(int id)
        {
            return (_context.ServiceInfos?.Any(e => e.ServiceInfoId == id)).GetValueOrDefault();
        }
    }
}
