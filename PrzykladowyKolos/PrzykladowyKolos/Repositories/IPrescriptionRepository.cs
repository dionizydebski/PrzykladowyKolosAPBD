using PrzykladowyKolos.DTOs;

namespace PrzykladowyKolos.Repositories;

public interface IPrescriptionRepository
{
    Task<IEnumerable<PrescriptionListDTO>> GetPrescriptionsAsync(string firstName);
    Task<PrescriptionDTO> AddPrescriptionAsync(PrescriptionDTO prescription);
}