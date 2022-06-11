using Microsoft.IdentityModel.Tokens;
using MiniFacebook.Data;
using MiniFacebook.Models;
using MiniFacebook.Models.users;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MiniFacebook.Repository
{
	public class JWTManagerRepository : IJWTManagerRepository
	{
		private readonly IConfiguration iconfiguration;
		public JWTManagerRepository(IConfiguration iconfiguration)
		{
			this.iconfiguration = iconfiguration;
		}
		public Tokens Authenticate(User users)
		{
			// Else we generate JSON Web Token
			var tokenHandler = new JwtSecurityTokenHandler();
			var tokenKey = Encoding.UTF8.GetBytes(iconfiguration["JWT:Key"]);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
			  {
				 new Claim("UserName", users.Username),
				 new Claim("userId", users.Id.ToString())

			  }),
				Expires = DateTime.UtcNow.AddMinutes(10),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return new Tokens { Token = tokenHandler.WriteToken(token) };

		}

		public  string GetUserId(ClaimsPrincipal claimsPrincipal)
		{
			var claim = claimsPrincipal.Claims.Where(c => c.Type == "userId").FirstOrDefault();
			return claim != null ? claim.Value : string.Empty;	
			
		}
	}
}
