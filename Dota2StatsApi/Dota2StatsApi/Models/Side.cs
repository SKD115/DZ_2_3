using System.ComponentModel.DataAnnotations;

namespace Dota2StatsApi.Models;

public class Side
{
    public int Id { get; set; }
    [Required, MaxLength(20)]
    public string Name { get; set; } = string.Empty;
    public double WinRate { get; set; }
    public ICollection<Match> Matches { get; set; } = new List<Match>();
}