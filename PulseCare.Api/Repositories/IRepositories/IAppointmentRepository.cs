using PulseCare.API.Data.Entities.Medical;

public interface IAppointmentRepository
{
    Task<IEnumerable<Appointment>> GetAppointmentsByPatientIdAsync(Guid patientId);
    Task<IEnumerable<Appointment>> GetAllAppointmentsAsync();
    Task<Appointment?> GetAppointmentByAppointmentIdAsync(Guid appointmentId);
    Task<Appointment> CreateAppointmentAsync(Appointment appointment);
    Task<Appointment?> UpdateAppointmentAsync(Appointment appointment);
    Task<bool> DeleteAppointmentAsync(Guid appointmentId);
    Task<IEnumerable<Appointment>> GetDoctorAppointmentsByClerkId(string clerkId);
    Task<IEnumerable<Appointment>> GetPatientAppointmentsByClerkId(string clerkId);
    Task<List<Appointment>> GetDoctorsAppointmentsAsync(string clerkId);
}