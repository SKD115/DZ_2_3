using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dota2StatsApi.Data;
using Dota2StatsApi.Models;

namespace Dota2StatsApi.Controllers;

[Route("api/[controller]"), ApiController]
public class MatchesController : ControllerBase
{
    private readonly DotaStatsDbContext _context;
    public MatchesController(DotaStatsDbContext context) => _context = context;

    [HttpGet] public async Task<ActionResult<IEnumerable<Match>>> GetMatches() => await _context.Matches.Include(m => m.Hero).Include(m => m.Side).ToListAsync();
    [HttpGet("{id}")] public async Task<ActionResult<Match>> GetMatch(int id) => await _context.Matches.Include(m => m.Hero).Include(m => m.Side).FirstOrDefaultAsync(m => m.Id == id) is Match m ? m : NotFound();
    [HttpPost] public async Task<ActionResult<Match>> CreateMatch(Match match) { if (!await _context.Heroes.AnyAsync(h => h.Id == match.HeroId)) return BadRequest("Hero does not exist."); if (!await _context.Sides.AnyAsync(s => s.Id == match.SideId)) return BadRequest("Side does not exist."); _context.Matches.Add(match); await _context.SaveChangesAsync(); return CreatedAtAction(nameof(GetMatch), new { id = match.Id }, match); }
    [HttpPut("{id}")] public async Task<IActionResult> UpdateMatch(int id, Match match) { if (id != match.Id) return BadRequest(); _context.Entry(match).State = EntityState.Modified; try { await _context.SaveChangesAsync(); } catch (DbUpdateConcurrencyException) { if (!_context.Matches.Any(m => m.Id == id)) return NotFound(); throw; } return NoContent(); }
    [HttpDelete("{id}")] public async Task<IActionResult> DeleteMatch(int id) { var match = await _context.Matches.FindAsync(id); if (match == null) return NotFound(); _context.Matches.Remove(match); await _context.SaveChangesAsync(); return NoContent(); }
}