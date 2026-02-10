using CrecheManagement.Domain.Dtos;

namespace CrecheManagement.Domain.Responses.Auth;

public record UserResponse(string Username, string Email, TokensDto Tokens);