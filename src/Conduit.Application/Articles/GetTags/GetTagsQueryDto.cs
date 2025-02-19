namespace Conduit.Application.Articles.GetTags;

public sealed class GetTagsQueryDto(string[] tags)
{
    public string[] Tags { get; init; } = tags;
}
