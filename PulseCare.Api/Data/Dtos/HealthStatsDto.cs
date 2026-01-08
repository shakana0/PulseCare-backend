using System.Text.Json.Serialization;
using PulseCare.API.Data.Enums;

public record HealthStatsDto
(
    Guid Id,

    [property: JsonConverter(typeof(JsonStringEnumConverter))]
    HealthStatType Type,

    string Value,
    string Unit,
    DateTime Date,

    [property: JsonConverter(typeof(JsonStringEnumConverter))]
    HealthStatusType Status
);

public record CreateHealtStatDto
(
    Guid Id,

    [property: JsonConverter(typeof(JsonStringEnumConverter))]
    HealthStatType Type,

    string Value,
    string Unit,
    DateTime Date,

    [property: JsonConverter(typeof(JsonStringEnumConverter))]
    HealthStatusType Status
);

public record UpdateHealthStatDto
(
    [property: JsonConverter(typeof(JsonStringEnumConverter))]
    HealthStatType? Type,

    string? Value,
    string? Unit,
    DateTime? Date,

    [property: JsonConverter(typeof(JsonStringEnumConverter))]
    HealthStatusType? Status
);