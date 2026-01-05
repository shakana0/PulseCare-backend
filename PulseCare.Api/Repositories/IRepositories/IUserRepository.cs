using PulseCare.API.Data.Entities.Users;

public interface IUserRepository
{
    Task AddDoctorAsync(Doctor newAdmin);
    Task AddUserAsync(User user);
    Task<Patient?> GetPatientFromUserAsync(Guid userId);
    Task<User?> GetUserAsync(string clerkId);
    Task RemovePatientAsync(Patient patient);
    Task<bool> IsExistingPatientAsync(string userId);
    Task<bool> IsExistingDoctorAsync(Guid userId);
    Task AddPatientAsync(Patient newPatient);
}