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
    [MaxLength(50)]
    public string Category { get; set; } = string.Empty;
    public double Cost { get; set; }

    public ICollection<Discount> Discounts { get; set; } = new HashSet<Discount>();
    public ICollection<SoftwareVersion> SoftwareVersions { get; set; } = new HashSet<SoftwareVersion>();
}