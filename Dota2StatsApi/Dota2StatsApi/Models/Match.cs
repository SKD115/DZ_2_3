using System.ComponentModel.DataAnnotations.Schema;

namespace Dota2StatsApi.Models;

public class Match
{
    public int Id { get; set; }
    public bool IsWin { get; set; }
    public DateTime MatchDate { get; set; } = DateTime.UtcNow;

    public int HeroId { get; set; }
    public int SideId { get; set; }

    [ForeignKey(nameof(HeroId))]
    public Hero? Hero { get; set; }
    [ForeignKey(nameof(SideId))]
    public Side? Side { get; set; }
}