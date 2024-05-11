using PrzykladowyKolos.DTOs;
using PrzykladowyKolos.Repositories;

namespace PrzykladowyKolos.Services;

public class PrescriptionService : IPrescriptionService
{
    private readonly IPrescriptionRepository _prescriptionRepository;

    public PrescriptionService(IPrescriptionRepository prescriptionRepository)
    {
        _prescriptionRepository = prescriptionRepository;
    }
    public async Task<IEnumerable<PrescriptionListDTO>> GetPrescriptions(string lastName)
    {
        return await _prescriptionRepository.GetPrescriptionsAsync(lastName);
    }

    public async Task<PrescriptionDTO> AddPrescription(PrescriptionDTO prescription)
    {
        if (prescription.DueDate <= prescription.Date)
            return null;
        return await _prescriptionRepository.AddPrescriptionAsync(prescription);
    }
}