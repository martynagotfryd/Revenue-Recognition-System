using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project.Models;

[Table("client")]
public class Client
{
    [Key] 
    public int Id { get; set; }
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    [MaxLength(100)]
    public string Mail { get; set; } = string.Empty;
    public int Phone { get; set; } 
    public int? KRS { get; set; } // only company
    public int? PESEL { get; set; } // only individual
    [MaxLength(200)]
    public string? LastName { get; set; } = string.Empty; // only individual
    public bool? IsDeleted { get; set; } = false; // only for individual

    public ICollection<Contract> Contracts { get; set; } = new HashSet<Contract>();
}