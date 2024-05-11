using System.Data.SqlClient;
using PrzykladowyKolos.DTOs;

namespace PrzykladowyKolos.Repositories;

public class PrescriptionRepository : IPrescriptionRepository
{
    private IConfiguration _configuration;

    public PrescriptionRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task<IEnumerable<PrescriptionListDTO>> GetPrescriptionsAsync(string lastName)
    {
        await using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await con.OpenAsync();
        
        await using var cmd = new SqlCommand();
        cmd.Connection = con;
        
        switch (lastName.ToLower())
        {
            case "none":
                cmd.CommandText = "SELECT p.IdPrescription, p.Date, p.DueDate, pa.LastName AS PatientLastName, d.LastName AS DoctorLastName FROM Prescription p JOIN Patient pa ON p.IdPatient = pa.IdPatient JOIN Doctor d ON p.IdDoctor = d.IdDoctor ORDER BY p.Date DESC";
                break;
            default:
                cmd.Parameters.AddWithValue("@lastName", lastName);
                cmd.CommandText = "SELECT p.IdPrescription, p.Date, p.DueDate, pa.LastName AS PatientLastName, d.LastName AS DoctorLastName FROM Prescription p JOIN Patient pa ON p.IdPatient = pa.IdPatient JOIN Doctor d ON p.IdDoctor = d.IdDoctor WHERE d.LastName = @lastName ORDER BY p.Date DESC";
                break;
        }
        
        var dr = await cmd.ExecuteReaderAsync();
        var prescriptions = new List<PrescriptionListDTO>();
        while (await dr.ReadAsync())
        {
            var prescription = new PrescriptionListDTO
            (
                IdPrescription: (int)dr["IdPrescription"],
                Date: (DateTime)dr["Date"],
                DueDate: (DateTime)dr["DueDate"],
                PatientLastName: dr["PatientLastName"].ToString(),
                DoctorLastName: dr["DoctorLastName"].ToString()
            );
            prescriptions.Add(prescription);
        }
        
        return prescriptions;
    }

    public async Task<PrescriptionDTO> AddPrescriptionAsync(PrescriptionDTO prescription)
    {
        await using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await con.OpenAsync();
        
        await using var cmd = new SqlCommand();
        cmd.Connection = con;
        
        cmd.CommandText = "INSERT INTO Prescription (Date, DueDate, IdPatient, IdDoctor) VALUES (@Date, @DueDate, @IdPatient, @IdDoctor)";
        cmd.Parameters.AddWithValue("@Date", prescription.Date);
        cmd.Parameters.AddWithValue("@DueDate", prescription.DueDate);
        cmd.Parameters.AddWithValue("@IdPatient", prescription.IdPatient);
        cmd.Parameters.AddWithValue("@IdDoctor", prescription.IdDoctor);
        
        var primaryKey = await cmd.ExecuteScalarAsync();
        
        var result = new PrescriptionDTO
        (
            IdPrescription: Convert.ToInt32(primaryKey),
            Date: prescription.Date,
            DueDate: prescription.DueDate,
            IdPatient: prescription.IdPatient,
            IdDoctor: prescription.IdDoctor
        );
        
        return result;
    }
}