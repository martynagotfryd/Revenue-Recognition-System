using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project.Models;

[Table("contract")]
public class Contract
{
    [Key]
    public int Id { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public DateTime UpgradesEnd { get; set; }
    public double Price { get; set; }
    public bool Signed { get; set; }
    public int IdClient { get; set; }
    public int IdSoftwareVersion { get; set; }

    [ForeignKey(nameof(IdClient))]
    public Client Client { get; set; } = null!;
    [ForeignKey(nameof(IdSoftwareVersion))]
    public SoftwareVersion SoftwareVersion { get; set; } = null!;

    public ICollection<Payment> Payments { get; set; } = new HashSet<Payment>();
}

