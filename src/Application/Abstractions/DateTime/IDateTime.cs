namespace Application.Abstractions.DateTime;

using System;

public interface IDateTime
{
    public DateTime UtcNow { get; }
}
