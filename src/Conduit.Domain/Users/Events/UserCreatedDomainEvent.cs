using Conduit.Domain.Abstractions;

namespace Conduit.Domain.Users.Events;

public sealed record UserCreatedDomainEvent(Guid Id) : IDomainEvent;
