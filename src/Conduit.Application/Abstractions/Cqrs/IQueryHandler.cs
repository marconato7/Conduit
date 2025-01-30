using FluentResults;
using MediatR;

namespace Conduit.Application.Abstractions.Cqrs;

public interface IQueryHandler<TQuery, TResponse>
    : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>;
