using Dingler.Auth.Models;
using Dingler.Data.Context;
using Dingler.Data.Entities.Credentials;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Dingler.Auth.AuthenticationService;

public class JwtAuthenticationService : IAuthenticationService
{
    private readonly HexCredentialsContext _context;
    private readonly RsaSecurityKey _signingKey;
    
    private const string ISSUER = "dingler-auth";
    private const string AUDIENCE = "dingler-game";
    
    public JwtAuthenticationService(HexCredentialsContext context, RsaSecurityKey signingKey)
    {
        _context = context;
        _signingKey = signingKey;
    }
    
	public async Task<Dictionary<string, string>> LoginAsync(LoginRequest request)
	{
            var dict = new Dictionary<string, string>();

            var userCredential = await _context.UserCredentials
                .Include(u => u.BannedUser)
                .Where(u => u.Email == request.User)
                .Select(u => new
                {
                    u.Email,
                    u.PasswordHash,
                    u.BannedUser
                })
                .FirstOrDefaultAsync().ConfigureAwait(false);
            if (userCredential is null)
            {
                // add in new user with password. In a real environment, this shouldn't be the functionality,
                // but I don't want to make a bespoke registration page right now.

                await RegisterAsync(request.User, request.Pass).ConfigureAwait(false);

                userCredential = await _context.UserCredentials
                .Include(u => u.BannedUser)
                .Where(u => u.Email == request.User)
                .Select(u => new
                {
                    u.Email,
                    u.PasswordHash,
                    u.BannedUser
                })
                .FirstAsync().ConfigureAwait(false);
            }

            if (!BCrypt.Net.BCrypt.EnhancedVerify(request.Pass, userCredential.PasswordHash))
            {
                dict["result"] = "Invalid username/password";

                return dict;
            }

            var bannedUser = userCredential.BannedUser;

            if (bannedUser != null && (bannedUser.DateOfBan + bannedUser.LengthOfBan) > DateTimeOffset.UtcNow.ToUnixTimeSeconds())
            {
                dict["result"] = "User is banned";

                return dict;
            }

            var now = DateTime.UtcNow;
            
            var claimsList = new List<Claim>()
            {
                new("email", userCredential.Email),
                new("username", userCredential.Email),
                new("region", request.Region),
                new("lang", request.Lang)
            };

            var signingCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.RsaSha256);
            var jwt = new JwtSecurityToken(
                issuer: ISSUER,
                audience: AUDIENCE,
                claims: claimsList,
                notBefore: now,
                expires: now.AddSeconds(300),
                signingCredentials: signingCredentials);

            var handler = new JwtSecurityTokenHandler();
            var signedToken = handler.WriteToken(jwt);
            
            dict["result"] = "success";
            dict["token"] = signedToken;

            return dict;
	}
    
    public async Task<bool> RegisterAsync(string email, string password)
    {
        var id = await _context.UserCredentials
            .Where(u => u.Email == email)
            .Select(u => u.Id)
            .FirstOrDefaultAsync()
            .ConfigureAwait(false);

        if (id > 0)
        {
            return false;
        }

        var hashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(password, 13);

        var newUser = new UserCredential
        {
            Email = email,
            PasswordHash = hashedPassword
        };

        await _context.UserCredentials.AddAsync(newUser).ConfigureAwait(false);

        await _context.SaveChangesAsync().ConfigureAwait(false);

        return true;
    }
}