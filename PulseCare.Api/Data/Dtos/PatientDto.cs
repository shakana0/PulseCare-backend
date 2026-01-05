public class PatientDto
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string? Phone { get; set; }
    public List<string> Conditions { get; set; } = new List<string>();
}