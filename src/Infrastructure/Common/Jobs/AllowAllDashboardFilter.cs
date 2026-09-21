namespace Infrastructure.Common.Jobs;

using Hangfire.Dashboard;

/// <summary>
/// Hangfire only serves the dashboard to local requests by default, which never matches once the
/// api is in a container. Left open in this build for the same reason Swagger is.
/// </summary>
public class AllowAllDashboardFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context) => true;
}
