using System.Security.Claims;

namespace LMS.BAL.Interfaces
{
	public interface ITokenService
	{
		public string? BuildToken(string? key, string? issuerer, string? audience, IEnumerable<Claim> claims);
		ClaimsPrincipal GetPrincipalFromExpiredToken(string? key, string? issuer, string? audience, string? token);
		public bool IsValidToken(string? key, string? issuerer, string? audience, string? token);
		public string GenerateRefreshToken();
	}
}
