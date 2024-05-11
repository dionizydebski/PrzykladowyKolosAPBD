using System.ComponentModel.DataAnnotations;

namespace PrzykladowyKolos.Models;

public class Patient
{
    [Required]
    public int IdPatient { get; set; }
    [MaxLength(100)]
    public string FirstName { get; set; }
    [MaxLength(100)]
    public string LastName { get; set; }
    public DateTime Birthdate { get; set; }
}