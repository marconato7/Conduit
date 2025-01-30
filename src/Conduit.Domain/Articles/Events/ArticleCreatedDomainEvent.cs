using Conduit.Domain.Abstractions;

namespace Conduit.Domain.Articles.Events;

public sealed record ArticleCreatedDomainEvent(Guid Id) : IDomainEvent;
