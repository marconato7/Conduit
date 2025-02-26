using Conduit.Domain.Articles;
using Conduit.Domain.Users;

namespace Conduit.Api.Extensions;

public sealed record ArticleInfo
(
    string       Title,
    string       Description,
    string       Body,
    User         Author,
    DateTime     CreatedAtUtc,
    List<string> TagsThatNeedToBeCreated,
    List<Tag>    ExistingTags
);
