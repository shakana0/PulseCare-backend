namespace PulseCare.API.Data.Dtos;

using PulseCare.API.Data.Enums;

public class UpdateAppointmentDto
{
    public DateTime Date { get; set; }
    public TimeSpan Time { get; set; }
    public AppointmentType Type { get; set; }
    public AppointmentStatusType Status { get; set; }
    public string? Reason { get; set; }
}
