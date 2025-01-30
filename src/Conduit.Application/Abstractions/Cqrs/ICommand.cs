using FluentResults;
using MediatR;

namespace Conduit.Application.Abstractions.Cqrs;

public interface ICommand<TResponse>
    : IRequest<Result<TResponse>>, IBaseCommand;

public interface IBaseCommand;
