namespace PrzykladowyKolos.DTOs;

public record PrescriptionDTO(int IdPrescription, DateTime Date, DateTime DueDate, int IdPatient, int IdDoctor);