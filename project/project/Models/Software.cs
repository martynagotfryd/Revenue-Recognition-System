using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project.Models;

[Table("software")]
public class Software
{
    [Key]
    public int Id { get; set; }
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;
    [MaxLength(100)]
    public string Version { get; set; } = string.Empty;
    [MaxLength(50)]
    public string Category { get; set; } = string.Empty;

    public ICollection<Discount> Discounts { get; set; } = new HashSet<Discount>();
    public ICollection<Contract> Contracts { get; set; } = new HashSet<Contract>();
}