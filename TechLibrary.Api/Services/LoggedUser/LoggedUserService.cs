using System.IdentityModel.Tokens.Jwt;
using TechLibrary.Api.Domain.Entities;
using TechLibrary.Api.Infraestructure.DataAccess;

namespace TechLibrary.Api.Services.LoggedUser;

public class LoggedUserService {
    private readonly HttpContext _httpContext;

    public LoggedUserService(HttpContext httpContext) {
        _httpContext = httpContext;
    }

    public User User(TechLibraryDbContext dbContext) {
        // Get the Authorization header from the request
        var authentication = _httpContext.Request.Headers["Authorization"].ToString();

        // Get the token from the Authorization header
        var token = authentication["Bearer ".Length..].Trim();

        // Create a new instance of the JwtSecurityTokenHandler
        var tokenHandler = new JwtSecurityTokenHandler();

        // Read the token
        var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

        // Get the identifier from the token
        var identifier = jwtSecurityToken.Claims.First(claim => claim.Type.Equals(JwtRegisteredClaimNames.Sub)).Value;

        // Parse the identifier to a Guid
        var userId = Guid.Parse(identifier);

        // Return the user from the database
        return dbContext.Users.First(user => user.Id.Equals(userId));
    }

}
