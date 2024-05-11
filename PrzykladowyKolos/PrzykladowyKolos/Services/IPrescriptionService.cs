using PrzykladowyKolos.DTOs;

namespace PrzykladowyKolos.Services;

public interface IPrescriptionService
{
    Task<IEnumerable<PrescriptionListDTO>> GetPrescriptions(string firstName);
    Task<PrescriptionDTO> AddPrescription(PrescriptionDTO prescription);
}