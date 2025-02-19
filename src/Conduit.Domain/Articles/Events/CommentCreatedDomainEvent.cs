using Conduit.Domain.Abstractions;

namespace Conduit.Domain.Articles.Events;

internal sealed record CommentCreatedDomainEvent(Guid Id) : IDomainEvent;
