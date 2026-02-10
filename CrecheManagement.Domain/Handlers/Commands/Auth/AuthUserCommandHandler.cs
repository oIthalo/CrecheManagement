using System.Net;
using CrecheManagement.Domain.Commands.Auth;
using CrecheManagement.Domain.Dtos;
using CrecheManagement.Domain.Exceptions;
using CrecheManagement.Domain.Interfaces.Encrypter;
using CrecheManagement.Domain.Interfaces.Repositories;
using CrecheManagement.Domain.Interfaces.Services;
using CrecheManagement.Domain.Messages;
using CrecheManagement.Domain.Models;
using CrecheManagement.Domain.Responses.Auth;
using CrecheManagement.Domain.Utils;
using MediatR;

namespace CrecheManagement.Domain.Handlers.Commands.Auth;

public class AuthUserCommandHandler : 
    IRequestHandler<RegisterUserCommand, BaseResponse<UserResponse>>,
    IRequestHandler<LoginUserCommand, BaseResponse<UserResponse>>
{
    private readonly IUsersRepository _usersRepository;
    private readonly ITextEncrypter _textEncrypter;
    private readonly ITokensService _tokensService;

    public AuthUserCommandHandler(
        IUsersRepository usersRepository,
        ITextEncrypter textEncrypter,
        ITokensService tokensService)
    {
        _usersRepository = usersRepository;
        _textEncrypter = textEncrypter;
        _tokensService = tokensService;
    }

    public async Task<BaseResponse<UserResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (await _usersRepository.ExistWithEmailAsync(request.Email))
            throw new CrecheManagementException(ReturnMessages.USER_ALREADY_REGISTERED, HttpStatusCode.Conflict);

        var user = new User()
        {
            KeepAlive = request.KeepAlive,
            Email = request.Email,
            Username = request.Username,
            LoginDate = DateTime.Now,
            Password = _textEncrypter.Encrypt(request.Password),
        };

        var tokens = GenerateTokens(user.Identifier);

        user.RefreshToken = tokens.RefreshToken;

        await _usersRepository.UpsertAsync(user);

        return new BaseResponse<UserResponse>()
        {
            StatusCode = HttpStatusCode.Created,
            Message = ReturnMessages.USER_REGISTERED_SUCCESSFULLY,
            Data = new UserResponse(user.Username, user.Email, tokens)
        };
    }

    public async Task<BaseResponse<UserResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _usersRepository.GetByEmailAsync(request.Email)
            ?? throw new CrecheManagementException(ReturnMessages.INVALID_CREDENTIALS, HttpStatusCode.Unauthorized);

        if (!_textEncrypter.IsValid(request.Password, user.Password))
            throw new CrecheManagementException(ReturnMessages.INVALID_CREDENTIALS, HttpStatusCode.Unauthorized);

        var tokens = GenerateTokens(user.Identifier);

        user.LoginDate = DateTime.Now;
        user.KeepAlive = request.KeepAlive;
        user.RefreshToken = tokens.RefreshToken;

        await _usersRepository.UpsertAsync(user);

        return new BaseResponse<UserResponse>()
        {
            StatusCode = HttpStatusCode.Created,
            Message = ReturnMessages.USER_LOGGED_SUCCESSFULLY,
            Data = new UserResponse(user.Username, user.Email, tokens)
        };
    }

    private TokensDto GenerateTokens(string identifier)
    {
        var accessToken = _tokensService.GenerateAccessToken(identifier);
        var refreshToken = _tokensService.GenerateRefreshToken();

        return new TokensDto(accessToken, refreshToken);
    }
}