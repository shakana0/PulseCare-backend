public record MedicationDto
(
    Guid? Id,
    string? Name,
    string? Dosage,
    string? Frequency,
    string? Instructions,
    int? TimesPerDay,
    DateTime? StartDate
);
