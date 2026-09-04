extern alias HexGame;
using Dingler.Server.Abstractions;
using Dingler.Data.Repositories;
using HexGame::Game.Shared.Network.SFS;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Dingler.Game.Protocol.Messages.Requests;
using HexGame::Game.Shared;
using Microsoft.Extensions.Logging;

namespace Dingler.Game.Services
{
    public sealed class SessionService : IAsyncStartupService
    {
        private readonly HttpClient _httpClient;
        private JsonWebKeySet? _cachedKeys;
        private readonly AccountRepository _gameDataService;
        private readonly ILogger<SessionService>? _logger;
        
        private const string ISSUER = "dingler-auth";
        private const string AUDIENCE = "dingler-game";

        public SessionService(IHttpClientFactory httpClientFactory, AccountRepository gameDataService,
            ILogger<SessionService>? logger = null)
        {
            _httpClient = httpClientFactory.CreateClient("AuthClient");
            _gameDataService = gameDataService;
            _logger = logger;
        }

        public async Task InitializeAsync(CancellationToken token)
        {
            try
            {
                var json = await _httpClient.GetStringAsync($".well-known/jwks.json", token);
                _cachedKeys = new JsonWebKeySet(json);
            }
            catch (Exception)
            {
                _logger?.LogError($"Could not reach Dingler.Auth at {_httpClient.BaseAddress}. " +
                    "Is it running? Check Auth:BaseUrl in appSettings of base server.");
                throw;
            }
        }

        public async Task<ClientConnection.AuthInfo> GetAuthenticationResponseAsync(AuthenticationRequestArg args)
        {
            if (_cachedKeys is null)
                throw new InvalidOperationException("Cached Keys must be initialized first");

            var handler = new JwtSecurityTokenHandler();
            handler.InboundClaimTypeMap.Clear();
            var parameters = new TokenValidationParameters()
            {
                ValidIssuer = ISSUER,
                ValidAudience = AUDIENCE,
                IssuerSigningKeys = _cachedKeys.GetSigningKeys(),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };

            var principal = handler.ValidateToken(args.Token, parameters, out _);
            
            var account = await _gameDataService.GetAccountByEmailAsync(principal.FindFirst(JwtRegisteredClaimNames.Email)?.Value ?? throw new InvalidOperationException("Claims has no email"))
                .ConfigureAwait(false);

            var accountId = (ulong)account.Id << 8 | (byte)UID.Type.ServicePlayer; 
            var gameId = (ulong)account.PlayerProfile?.Id! << 8 | (byte)UID.Type.ServicePlayer; 
            return new ClientConnection.AuthInfo 
            {
                Success = true, 
                UserName = account.PlayerProfile?.Username ?? "", 
                SAuthID = accountId.ToString(), 
                authID = new UID(accountId), 
                SReckID = gameId.ToString(), 
                reckID = new UID(gameId) 
            };
        }
    }
}
