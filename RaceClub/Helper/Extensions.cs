using System.Security.Claims;

namespace RaceClub.Helper
{
    public static class Extensions
    {
        public static string GetUserId(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}
