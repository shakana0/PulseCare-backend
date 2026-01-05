using PulseCare.API.Data.Entities.Medical;

public interface IAppointmentRepository
{
    Task<IEnumerable<Appointment>> GetAppointmentsById(Guid id);
}