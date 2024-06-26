using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project.Models;

[Table("software_version")]
public class SoftwareVersion
{
    [Key]
    public int Id { get; set; }
    [MaxLength(200)]
    public string Version { get; set; } = string.Empty;
    public int IdSoftware { get; set; }

    [ForeignKey(nameof(IdSoftware))] 
    public Software Software { get; set; } = null!;

    public ICollection<Contract> Contracts { get; set; } = new HashSet<Contract>();
}