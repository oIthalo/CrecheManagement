using CrecheManagement.Domain.Responses.Auth;
using CrecheManagement.Domain.Utils;
using MediatR;

namespace CrecheManagement.Domain.Commands.Auth;

public class LoginUserCommand : IRequest<BaseResponse<UserResponse>>
{
    public string Email { get; set; }
    public string Password { get; set; }
    public bool KeepAlive { get; set; }
}