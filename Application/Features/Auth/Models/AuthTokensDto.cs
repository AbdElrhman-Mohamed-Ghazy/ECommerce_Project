namespace Application.Features.Auth.Models
{
    public record AuthTokensDto(string AccessToken, string RefreshToken);
}
