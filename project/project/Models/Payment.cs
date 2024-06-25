using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project.Models;

[Table("payment")]
public class Payment
{
    [Key]
    public int Id { get; set; }
    public double Value { get; set; }
    public int IdContract { get; set; }

    [ForeignKey(nameof(IdContract))] 
    public Contract Contract { get; set; } = null!;
}