using System.ComponentModel.DataAnnotations;

namespace PrzykladowyKolos.Models;

public class Doctor
{
    [Required]
    public int IdDoctor { get; set; }
    [MaxLength(100)]
    public string FirstName { get; set; }
    [MaxLength(100)]
    public string LastName { get; set; }
    [MaxLength(100)]
    public string Email { get; set; }
}