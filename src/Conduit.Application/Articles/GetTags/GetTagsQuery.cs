using Conduit.Application.Abstractions.Cqrs;

namespace Conduit.Application.Articles.GetTags;

public sealed record GetTagsQuery()
    : IQuery<GetTagsQueryDto>;
