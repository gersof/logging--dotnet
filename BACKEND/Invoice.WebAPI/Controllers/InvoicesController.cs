using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Invoice.WebAPI.Models;
using Invoice.WebAPI.Dtos;

namespace Invoice.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly InvoiceDb_DevContext _context;

        public InvoicesController(InvoiceDb_DevContext context)
        {
            _context = context;
        }

        [HttpPost("create-full-invoice")]
        public async Task<InvoiceDto> CreateFullInvoice(InvoiceDto invoice)
        {
            var invoiceResult = new InvoiceDto();
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var invoicetable = new Invoice.WebAPI.Models.Invoice();
                    invoicetable.Date = invoice.Date;
                    invoicetable.ClientName = invoice.ClientName;
                    invoicetable.EmployeeId = invoice.EmployeeId;
                    _context.Invoice.Add(invoicetable);
                    _context.SaveChanges();

                    foreach(var item in invoice.InvoiceDetail)
                    {
                        var detailInvoice = new InvoiceDetail();
                        detailInvoice.InvoiceId = invoicetable.Id;
                        detailInvoice.Price = item.Price;
                        detailInvoice.ProductId = item.ProductId;
                        detailInvoice.Quantity = item.Quantity;
                        _context.InvoiceDetail.Add(detailInvoice);
                    }
                    _context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    // TODO: Handle failure
                }

            }

            return invoiceResult;
        }

        // GET: api/Invoices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Invoice.WebAPI.Models.Invoice>>> GetInvoice()
        {
            return await _context.Invoice.ToListAsync();
        }

        // GET: api/Invoices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Invoice.WebAPI.Models.Invoice>> GetInvoice(int id)
        {
            var invoice = await _context.Invoice.FindAsync(id);

            if (invoice == null)
            {
                return NotFound();
            }

            return invoice;
        }

        // PUT: api/Invoices/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInvoice(int id, Invoice.WebAPI.Models.Invoice invoice)
        {
            if (id != invoice.Id)
            {
                return BadRequest();
            }

            _context.Entry(invoice).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvoiceExists(id))
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

        // POST: api/Invoices
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Invoice.WebAPI.Models.Invoice>> PostInvoice(Invoice.WebAPI.Models.Invoice invoice)
        {
            _context.Invoice.Add(invoice);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInvoice", new { id = invoice.Id }, invoice);
        }

        // DELETE: api/Invoices/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Invoice.WebAPI.Models.Invoice>> DeleteInvoice(int id)
        {
            var invoice = await _context.Invoice.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }

            _context.Invoice.Remove(invoice);
            await _context.SaveChangesAsync();

            return invoice;
        }

        private bool InvoiceExists(int id)
        {
            return _context.Invoice.Any(e => e.Id == id);
        }
    }
}
