using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WInnovator.Data;
using WInnovator.Models;

namespace WInnovator.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class DesignShopController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DesignShopController> _logger;

        public DesignShopController(ApplicationDbContext context, ILogger<DesignShopController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/DesignShop
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DesignShop>>> GetDesignShop()
        {
            return await _context.DesignShop.Include(shops => shops.UploadedImages).ToListAsync();
        }

        // GET: api/DesignShop/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DesignShop>> GetDesignShop(Guid id)
        {
            var designShop = await _context.DesignShop.FindAsync(id);

            if (designShop == null)
            {
                return NotFound();
            }

            return designShop;
        }

        // PUT: api/DesignShop/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutDesignShop(Guid id, DesignShop designShop)
        //{
        //    if (id != designShop.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(designShop).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!DesignShopExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/DesignShop
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost("create")]
        public async Task<ActionResult<DesignShop>> CreateDesignShop()
        {
            // Currently, we only need an empty DesignShop so we'll create it here.
            _logger.LogTrace("Creating new Design Shop.");
            DesignShop designShop = new DesignShop();
            _context.DesignShop.Add(designShop);
            await _context.SaveChangesAsync();
            _logger.LogTrace($"New Design Shop created with id { designShop.Id }");

            return CreatedAtAction("GetDesignShop", new { id = designShop.Id }, designShop);
        }

        // DELETE: api/DesignShop/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<DesignShop>> DeleteDesignShop(Guid id)
        //{
        //    var designShop = await _context.DesignShop.FindAsync(id);
        //    if (designShop == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.DesignShop.Remove(designShop);
        //    await _context.SaveChangesAsync();

        //    return designShop;
        //}

        private bool DesignShopExists(Guid id)
        {
            return _context.DesignShop.Any(e => e.Id == id);
        }
    }
}
