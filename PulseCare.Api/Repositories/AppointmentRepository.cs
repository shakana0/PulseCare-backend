using PulseCare.API.Data.Entities.Medical;

public class AppointmentRepository : IAppointmentRepository
{
    public Task<IEnumerable<Appointment>> GetAppointmentsById(Guid id)
    {
        throw new NotImplementedException();
    }

}