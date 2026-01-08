using PulseCare.API.Data.Entities.Users;

public interface IUserRepository
{
    Task AddDoctorAsync(Doctor newAdmin);
    Task AddUserAsync(User user);
    Task<Patient?> GetPatientFromUserAsync(Guid userId);
    Task<Doctor?> GetDoctorWithClerkIdAsync(string clerkId);
    Task<User?> GetUserAsync(string clerkId);
    Task RemovePatientAsync(Patient patient);
    Task<bool> IsExistingPatientAsync(string userId);
    Task<bool> IsExistingDoctorAsync(Guid userId);
    Task AddPatientAsync(Patient newPatient);
    Task<User?> GetUserByPatientIdAsync(Guid patientId);
    Task<User?> GetUserByDoctorIdAsync(Guid doctorId);
    Task<Doctor?> GetDoctorFromUserAsync(Guid doctorId);
    Task<Patient?> GetPatientAsync(Guid userId);
}