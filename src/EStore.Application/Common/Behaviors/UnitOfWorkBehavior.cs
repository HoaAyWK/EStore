using System.Transactions;
using EStore.Application.Common.Interfaces.Persistence;
using MediatR;

namespace EStore.Application.Common.Behaviors;

public sealed class UnitOfWorkBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public UnitOfWorkBehavior(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (IsNotCommand())
        {
            return await next();
        }

        using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {

            var result = await next();

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            transactionScope.Complete();

            return result;
        }
    }

    private static bool IsNotCommand()
    {
        return !typeof(TRequest).Name.EndsWith("Command");
    }
}
