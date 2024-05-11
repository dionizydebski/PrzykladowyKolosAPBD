namespace PrzykladowyKolos.DTOs;

public record PrescriptionListDTO(int IdPrescription, DateTime Date, DateTime DueDate, string PatientLastName, string DoctorLastName);