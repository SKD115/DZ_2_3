using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dota2StatsApi.Data;
using Dota2StatsApi.Models;

namespace Dota2StatsApi.Controllers;

[Route("api/[controller]"), ApiController]
public class HeroesController : ControllerBase
{
    private readonly DotaStatsDbContext _context;
    public HeroesController(DotaStatsDbContext context) => _context = context;

    [HttpGet] public async Task<ActionResult<IEnumerable<Hero>>> GetHeroes() => await _context.Heroes.ToListAsync();
    [HttpGet("{id}")] public async Task<ActionResult<Hero>> GetHero(int id) => await _context.Heroes.FindAsync(id) is Hero h ? h : NotFound();
    [HttpPost] public async Task<ActionResult<Hero>> CreateHero(Hero hero) { _context.Heroes.Add(hero); await _context.SaveChangesAsync(); return CreatedAtAction(nameof(GetHero), new { id = hero.Id }, hero); }
    [HttpPut("{id}")] public async Task<IActionResult> UpdateHero(int id, Hero hero) { if (id != hero.Id) return BadRequest(); _context.Entry(hero).State = EntityState.Modified; try { await _context.SaveChangesAsync(); } catch (DbUpdateConcurrencyException) { if (!_context.Heroes.Any(h => h.Id == id)) return NotFound(); throw; } return NoContent(); }
    [HttpDelete("{id}")] public async Task<IActionResult> DeleteHero(int id) { var hero = await _context.Heroes.FindAsync(id); if (hero == null) return NotFound(); _context.Heroes.Remove(hero); await _context.SaveChangesAsync(); return NoContent(); }
}