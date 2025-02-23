using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TechLibrary.Api.Domain.Entities;

namespace TechLibrary.Api.Infraestructure.Security.Tokens.Access; 
public class JwtTokenGenerator {

    public string Generate(User user) {
        // Create the claims
        var claims = new List<Claim>() {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString())
        };

        // Define the descriptors of token
        var tokenDescriptor = new SecurityTokenDescriptor {
            Expires = DateTime.UtcNow.AddHours(2), // Expires in 2 hours
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = new SigningCredentials(SecurityKey(), SecurityAlgorithms.HmacSha256Signature),
        };

        // Create the token
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(securityToken);
    }

    private SymmetricSecurityKey SecurityKey() {
        var signingKey = "mtQYyVTU97wuKgk1ZvNCbS8pDtyONVru";

        var symmetricKey = Encoding.UTF8.GetBytes(signingKey);

        return new SymmetricSecurityKey(symmetricKey);
    }

}
