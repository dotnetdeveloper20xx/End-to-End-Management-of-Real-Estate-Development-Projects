namespace BuildEstate.Application.Abstractions;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}