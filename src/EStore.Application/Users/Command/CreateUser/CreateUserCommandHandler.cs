using ErrorOr;
using EStore.Application.Common.Interfaces.Persistence;
using EStore.Domain.Common.Errors;
using EStore.Domain.UserAggregate;
using EStore.Domain.UserAggregate.Repositories;
using MediatR;

namespace EStore.Application.Users.Command.CreateUser;

public class CreateUserCommandHandler
    : IRequestHandler<CreateUserCommand, ErrorOr<User>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<User>> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        var createUserResult = User.Create(request.Email, request.FirstName, request.LastName);

        if (createUserResult.IsError)
        {
            return createUserResult.Errors;
        }

        var userWithEmailExisted = await _userRepository.GetByEmailAsync(request.Email);

        if (userWithEmailExisted is not null)
        {
            return Errors.User.DuplicateEmail;
        }

        var user = createUserResult.Value;

        await _userRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return user;
    }
}
