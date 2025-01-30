using FluentResults;
using MediatR;

namespace Conduit.Application.Abstractions.Cqrs;

public interface ICommandHandler<TCommand, TResponse>
    : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>;
