namespace Conduit.Application.Articles.Shared;

public sealed class ConduitError(string message)
{
    public string Message { get; init; } = message;
}
