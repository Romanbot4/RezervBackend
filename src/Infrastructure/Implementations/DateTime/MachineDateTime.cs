namespace Infrastructure.Implementations.DateTime;

using System;
using Application.Abstractions.DateTime;

public class MachineDateTime : IDateTime
{
    public DateTime UtcNow { get; } = DateTime.UtcNow;
}
