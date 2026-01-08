public class AdminDashboardDto
{
    public int TotalPatients { get; set; }
    public int TodayAppointments { get; set; }
    public List<PatientDto> RecentPatients { get; set; } = new List<PatientDto>();
    public List<AppointmentDto> UpcomingAppointments { get; set; } = new List<AppointmentDto>();
}