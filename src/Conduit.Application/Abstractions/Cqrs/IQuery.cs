using FluentResults;
using MediatR;

namespace Conduit.Application.Abstractions.Cqrs;

public interface IQuery<TResponse>
    : IRequest<Result<TResponse>>;
