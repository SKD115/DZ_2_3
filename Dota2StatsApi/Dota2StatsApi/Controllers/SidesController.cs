using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dota2StatsApi.Data;
using Dota2StatsApi.Models;

namespace Dota2StatsApi.Controllers;

[Route("api/[controller]"), ApiController]
public class SidesController : ControllerBase
{
    private readonly DotaStatsDbContext _context;
    public SidesController(DotaStatsDbContext context) => _context = context;

    [HttpGet] public async Task<ActionResult<IEnumerable<Side>>> GetSides() => await _context.Sides.ToListAsync();
    [HttpGet("{id}")] public async Task<ActionResult<Side>> GetSide(int id) => await _context.Sides.FindAsync(id) is Side s ? s : NotFound();
    [HttpPost] public async Task<ActionResult<Side>> CreateSide(Side side) { _context.Sides.Add(side); await _context.SaveChangesAsync(); return CreatedAtAction(nameof(GetSide), new { id = side.Id }, side); }
    [HttpPut("{id}")] public async Task<IActionResult> UpdateSide(int id, Side side) { if (id != side.Id) return BadRequest(); _context.Entry(side).State = EntityState.Modified; try { await _context.SaveChangesAsync(); } catch (DbUpdateConcurrencyException) { if (!_context.Sides.Any(s => s.Id == id)) return NotFound(); throw; } return NoContent(); }
    [HttpDelete("{id}")] public async Task<IActionResult> DeleteSide(int id) { var side = await _context.Sides.FindAsync(id); if (side == null) return NotFound(); _context.Sides.Remove(side); await _context.SaveChangesAsync(); return NoContent(); }
}