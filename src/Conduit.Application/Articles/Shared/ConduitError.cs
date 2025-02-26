namespace Conduit.Application.Articles.Shared;

public sealed class ConduitErrorContainer(List<ConduitError> errors)
{
    public List<ConduitError> Errors { get; init; } = errors;
}

public sealed class ConduitError(string message)
{
    public string Message { get; init; } = message;
}

public static class ArticlesErrors
{
    public static ConduitError NotFound
        => new("article not found");
}
