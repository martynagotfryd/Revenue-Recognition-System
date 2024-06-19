using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project.Models;

[Table("contract")]
public class Contract
{
    [Key]
    public int Id { get; set; }
    public int ClientId { get; set; }
    public int SoftwareId { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public double Price { get; set; }
    public bool Signed { get; set; }

    [ForeignKey(nameof(ClientId))]
    public Client Client { get; set; } = null!;
    [ForeignKey(nameof(SoftwareId))]
    public Software Software { get; set; } = null!;

    public ICollection<Payment> Payments { get; set; } = null!;
}

public class Payment
{
    public int Id { get; set; }
    public double Value { get; set; }
}