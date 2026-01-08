using PulseCare.API.Data.Entities.Users;

public interface IDoctorRepository
{
    Task<IEnumerable<Doctor>> GetAllDoctorsAsync();

}