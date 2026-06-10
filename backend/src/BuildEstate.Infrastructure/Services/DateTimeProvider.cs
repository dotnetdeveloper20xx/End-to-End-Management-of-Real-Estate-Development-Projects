using BuildEstate.Application.Abstractions;

namespace BuildEstate.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}