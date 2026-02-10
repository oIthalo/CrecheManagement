using CrecheManagement.Domain.Responses.Auth;
using CrecheManagement.Domain.Utils;
using MediatR;

namespace CrecheManagement.Domain.Commands.Auth;

public class RegisterUserCommand : IRequest<BaseResponse<UserResponse>>
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool KeepAlive { get; set; }
}