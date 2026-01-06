using PulseCare.API.Data.Entities.Users;

public interface IPatientRepository
{
    Task<IEnumerable<Patient>> GetAllPatientsAsync();
    Task<Patient?> GetPatientByIdAsync(Guid patientId);
    Task<Patient?> GetPatientByClerkIdAsync(string clerkId);
}