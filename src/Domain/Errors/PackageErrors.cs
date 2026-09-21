using Core.Exception;
using Core.Exception.NetworkException;

namespace Domain.Errors;

public static class PackageErrors
{
    public static CoreException PackageNotActive() =>
        new UnprocessableException("This package is no longer available for purchase");
}
