using Dingler.Auth.Models;
using Dingler.Data.Context;
using Dingler.Data.Entities.Credentials;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Dingler.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Dingler.Auth.AuthenticationService;

public class JwtAuthenticationService : IAuthenticationService
{
    private readonly SqliteWriter<HexCredentialsContext> _sqliteWriter;
    private readonly IDbContextFactory<HexCredentialsContext> _factory;
    private readonly RsaSecurityKey _signingKey;
    
    private const string ISSUER = "dingler-auth";
    private const string AUDIENCE = "dingler-game";
    
    public JwtAuthenticationService(SqliteWriter<HexCredentialsContext> sqliteWriter, IDbContextFactory<HexCredentialsContext> factory, RsaSecurityKey signingKey)
    {
        _sqliteWriter = sqliteWriter;
        _factory = factory;
        _signingKey = signingKey;
    }
    
	public async Task<Dictionary<string, string>> LoginAsync(LoginRequest request)
	{
            var dict = new Dictionary<string, string>();

            var context = await _factory.CreateDbContextAsync();
            
            var userCredential = await context.UserCredentials
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

                var hashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Pass, 13);
                if (!await RegisterAsync(request.User, hashedPassword).ConfigureAwait(false))
                {
                    dict["result"] = "Could not register user";
                    return dict;
                }

                userCredential = new
                    { Email = request.User, PasswordHash = hashedPassword, BannedUser = (BannedUser?)null };
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
    
    public Task<bool> RegisterAsync(string email, string hashedPassword)
    {
        var newUser = new UserCredential
        {
            Email = email,
            PasswordHash = hashedPassword
        };

        return _sqliteWriter.EnqueueWriteAsync(async context =>
        {
            var id = await context.UserCredentials
                .Where(u => u.Email == email)
                .Select(u => u.Id)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            if (id > 0)
                return false;

            await context.UserCredentials.AddAsync(newUser).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);
            return true;
        });
    }
}