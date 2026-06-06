using System.ComponentModel.DataAnnotations;

namespace Dota2StatsApi.Models;

public class Hero
{
    public int Id { get; set; }
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    [Required, MaxLength(20)]
    public string Attribute { get; set; } = string.Empty;
    public double WinRate { get; set; }
    public ICollection<Match> Matches { get; set; } = new List<Match>();
}