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
    public class SubscriptionPlansController : ControllerBase
    {
        private readonly MidprojectDbContext _context;

        public SubscriptionPlansController(MidprojectDbContext context)
        {
            _context = context;
        }

        // GET: api/SubscriptionPlans
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubscriptionPlan>>> GetSubscriptionPlans()
        {
          if (_context.SubscriptionPlans == null)
          {
              return NotFound();
          }
            return await _context.SubscriptionPlans.ToListAsync();
        }

        // GET: api/SubscriptionPlans/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SubscriptionPlan>> GetSubscriptionPlan(int id)
        {
          if (_context.SubscriptionPlans == null)
          {
              return NotFound();
          }
            var subscriptionPlan = await _context.SubscriptionPlans.FindAsync(id);

            if (subscriptionPlan == null)
            {
                return NotFound();
            }

            return subscriptionPlan;
        }

        // PUT: api/SubscriptionPlans/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSubscriptionPlan(int id, SubscriptionPlan subscriptionPlan)
        {
            if (id != subscriptionPlan.SubscriptionPlanId)
            {
                return BadRequest();
            }

            _context.Entry(subscriptionPlan).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubscriptionPlanExists(id))
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

        // POST: api/SubscriptionPlans
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SubscriptionPlan>> PostSubscriptionPlan(SubscriptionPlan subscriptionPlan)
        {
          if (_context.SubscriptionPlans == null)
          {
              return Problem("Entity set 'MidprojectDbContext.SubscriptionPlans'  is null.");
          }
            _context.SubscriptionPlans.Add(subscriptionPlan);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSubscriptionPlan", new { id = subscriptionPlan.SubscriptionPlanId }, subscriptionPlan);
        }

        // DELETE: api/SubscriptionPlans/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubscriptionPlan(int id)
        {
            if (_context.SubscriptionPlans == null)
            {
                return NotFound();
            }
            var subscriptionPlan = await _context.SubscriptionPlans.FindAsync(id);
            if (subscriptionPlan == null)
            {
                return NotFound();
            }

            _context.SubscriptionPlans.Remove(subscriptionPlan);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SubscriptionPlanExists(int id)
        {
            return (_context.SubscriptionPlans?.Any(e => e.SubscriptionPlanId == id)).GetValueOrDefault();
        }
    }
}
