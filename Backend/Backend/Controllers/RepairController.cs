using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;


namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepairController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RepairController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{ctel}")]
        public async Task<ActionResult<IEnumerable<Repair>>> GetRepairsByPhone([FromRoute] string ctel)
        {
            // 現在 ctel 會正確拿到 "22716835"
            if (string.IsNullOrEmpty(ctel))
            {
                return BadRequest("必須提供電話號碼進行查詢。");
            }

            var repairs = await _context.Repairs
                .Where(r => r.Rtel == ctel)
                .OrderByDescending(r => r.Rdate1)
                .ToListAsync();

            return Ok(repairs);
        }
    }
}
