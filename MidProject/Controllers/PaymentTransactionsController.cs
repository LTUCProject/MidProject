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
    public class PaymentTransactionsController : ControllerBase
    {
        private readonly MidprojectDbContext _context;

        public PaymentTransactionsController(MidprojectDbContext context)
        {
            _context = context;
        }

        // GET: api/PaymentTransactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentTransaction>>> GetPaymentTransactions()
        {
          if (_context.PaymentTransactions == null)
          {
              return NotFound();
          }
            return await _context.PaymentTransactions.ToListAsync();
        }

        // GET: api/PaymentTransactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentTransaction>> GetPaymentTransaction(int id)
        {
          if (_context.PaymentTransactions == null)
          {
              return NotFound();
          }
            var paymentTransaction = await _context.PaymentTransactions.FindAsync(id);

            if (paymentTransaction == null)
            {
                return NotFound();
            }

            return paymentTransaction;
        }

        // PUT: api/PaymentTransactions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPaymentTransaction(int id, PaymentTransaction paymentTransaction)
        {
            if (id != paymentTransaction.PaymentTransactionId)
            {
                return BadRequest();
            }

            _context.Entry(paymentTransaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentTransactionExists(id))
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

        // POST: api/PaymentTransactions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PaymentTransaction>> PostPaymentTransaction(PaymentTransaction paymentTransaction)
        {
          if (_context.PaymentTransactions == null)
          {
              return Problem("Entity set 'MidprojectDbContext.PaymentTransactions'  is null.");
          }
            _context.PaymentTransactions.Add(paymentTransaction);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPaymentTransaction", new { id = paymentTransaction.PaymentTransactionId }, paymentTransaction);
        }

        // DELETE: api/PaymentTransactions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaymentTransaction(int id)
        {
            if (_context.PaymentTransactions == null)
            {
                return NotFound();
            }
            var paymentTransaction = await _context.PaymentTransactions.FindAsync(id);
            if (paymentTransaction == null)
            {
                return NotFound();
            }

            _context.PaymentTransactions.Remove(paymentTransaction);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PaymentTransactionExists(int id)
        {
            return (_context.PaymentTransactions?.Any(e => e.PaymentTransactionId == id)).GetValueOrDefault();
        }
    }
}
