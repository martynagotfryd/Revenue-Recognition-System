namespace project.DTOs;

public class NewClientDTO
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Mail { get; set; } = string.Empty;
    public int Phone { get; set; } 
    public int? KRS { get; set; } // only company
    public int? PESEL { get; set; } // only individual
    public string? LastName { get; set; } = string.Empty;
}