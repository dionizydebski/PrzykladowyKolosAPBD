using Microsoft.AspNetCore.Mvc;
using PrzykladowyKolos.DTOs;
using PrzykladowyKolos.Services;

namespace PrzykladowyKolos.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PrescriptionController : ControllerBase
{
    private IPrescriptionService _prescriptionService;

    public PrescriptionController(IPrescriptionService prescriptionService)
    {
        _prescriptionService = prescriptionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPrescriptions(string lastName = "none")
    {
        var prescriptions = await _prescriptionService.GetPrescriptions(lastName);
        return Ok(prescriptions);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddPrescription(PrescriptionDTO prescription)
    {
        var result = await _prescriptionService.AddPrescription(prescription);
        if (result == null)
            return StatusCode(StatusCodes.Status400BadRequest);
        return Ok(result);
    }
}