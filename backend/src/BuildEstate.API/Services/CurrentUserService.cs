using BuildEstate.Application.Abstractions;

namespace BuildEstate.API.Services;

public class CurrentUserService : ICurrentUserService
{
    public string? UserId => "SYSTEM";

    public string? UserName => "System User";

    public string? Email => "system@buildestate.local";

    public bool IsAuthenticated => true;
}