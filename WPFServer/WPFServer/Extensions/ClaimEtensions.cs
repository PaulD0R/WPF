using System.Security.Claims;

namespace WPFServer.Extensions
{
    public static class ClaimEtensions
    {
        public static string? GetUserName(this ClaimsPrincipal principal)
        {
            return principal.Claims.FirstOrDefault(x =>
                x.Type.Equals(ClaimTypes.GivenName))?.Value;
        }

        public static string? GetEmail(this ClaimsPrincipal principal)
        {
            return principal.Claims.FirstOrDefault(x =>
                x.Type.Equals(ClaimTypes.Email))?.Value;
        }

        public static string? GetId(this ClaimsPrincipal principal)
        {
            return principal.Claims.FirstOrDefault(x =>
                x.Type.Equals(ClaimTypes.NameIdentifier))?.Value;
        }
    }
}
