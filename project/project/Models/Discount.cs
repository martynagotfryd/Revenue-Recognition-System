using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project.Models;

[Table("discount")]
public class Discount
{
    [Key]
    public int Id { get; set; }
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    public double Value { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public int IdSoftware { get; set; }

    [ForeignKey(nameof(IdSoftware))]
    public Software Software { get; set; } = null!;

}