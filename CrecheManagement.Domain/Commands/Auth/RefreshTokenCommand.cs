using CrecheManagement.Domain.Dtos;
using CrecheManagement.Domain.Utils;
using MediatR;

namespace CrecheManagement.Domain.Commands.Auth;

public class RefreshTokenCommand : IRequest<BaseResponse<TokensDto>>
{
    public string RefreshToken { get; set; }
}