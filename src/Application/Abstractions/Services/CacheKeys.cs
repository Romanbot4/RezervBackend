namespace Application.Abstractions.Services;

using System;

public static class CacheKeys
{
    public const string TimetablePrefix = "timetable:";

    public static readonly TimeSpan TimetableTtl = TimeSpan.FromSeconds(60);

    public static string Timetable(Guid? businessId, DateOnly? date) =>
        $"{TimetablePrefix}{businessId?.ToString() ?? "all"}:{date?.ToString("yyyy-MM-dd") ?? "all"}";
}
