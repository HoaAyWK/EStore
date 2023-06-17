using ErrorOr;
using EStore.Application.Common.Dtos;
using EStore.Application.Common.Interfaces.Authentication;
using EStore.Application.Common.Interfaces.Services;
using EStore.Domain.Common.Errors;
using EStore.Domain.UserAggregate;
using EStore.Domain.UserAggregate.Repositories;
using EStore.Infrastructure.Authentication;
using EStore.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace EStore.Infrastructure.Services;

internal sealed class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;

    public AuthenticationService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IJwtTokenGenerator jwtTokenGenerator,
        IUserRepository userRepository)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    }

    public async Task<ErrorOr<AuthenticationResult>> LoginAsync(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email);

        if (user is null)
        {
            return Errors.Authentication.InvalidCredentials;
        }

        var appUser = await _userManager.FindByEmailAsync(email);

        if (appUser is null)
        {
            return Errors.Authentication.InvalidCredentials;
        }

        var validPassword = await _userManager.CheckPasswordAsync(appUser, password);

        if (validPassword)
        {
            var generateTokenResult = await _jwtTokenGenerator.GenerateTokenAsync(user.Id.Value.ToString());

            if (generateTokenResult.IsError)
            {
                return generateTokenResult.Errors;
            }

            return new AuthenticationResult()
            {
                User = user,
                Token = generateTokenResult.Value
            };
        }

        return Errors.Authentication.InvalidCredentials;
    }

    public async Task<ErrorOr<AuthenticationResult>> RegisterAsync(User user, string password)
    {
        var userId = user.Id.Value.ToString();

        var appUser = new ApplicationUser
        {
            Id = userId,
            Email = user.Email,
            UserName = user.Email,
            EmailConfirmed = true
        };

        var createAppUserResult = await _userManager.CreateAsync(appUser, password);

        if (createAppUserResult.Succeeded)
        {
            await _userManager.AddToRoleAsync(appUser, Roles.Customer);
            var generateTokenResult = await _jwtTokenGenerator.GenerateTokenAsync(userId);

            if (generateTokenResult.IsError)
            {
                return generateTokenResult.Errors;
            }

            return new AuthenticationResult()
            {
                User = user,
                Token = generateTokenResult.Value
            };
        }

        List<Error> errors = new List<Error>();

        foreach (var error in createAppUserResult.Errors)
        {
            errors.Add(Error.Validation(
                code: error.Code,
                description: error.Description));
        }

        return errors;
    }
}
