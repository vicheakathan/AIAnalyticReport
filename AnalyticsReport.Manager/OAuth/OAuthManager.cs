using AnalyticsReport.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AnalyticsReport.Manager
{
    public class OAuthManager
    {
        private readonly UserManager<ApplicationUser> userManager;

        private readonly IConfiguration configuration;

        private readonly SignInManager<ApplicationUser> signInManager;

        private readonly IHttpContextAccessor httpContext;

        public OAuthManager(UserManager<ApplicationUser> userManager, IConfiguration configuration, SignInManager<ApplicationUser> signInManager, IHttpContextAccessor httpContext)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.signInManager = signInManager;
            this.httpContext = httpContext;
        }

        public async Task<ApplicationUser> Create(BaseCredential credentail)
        {
            ApplicationUser user = new ApplicationUser()
            {
                UserName = credentail.Username,
            };

            IdentityResult result = await userManager.CreateAsync(user, credentail.Password);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException(result.Errors.First().Description);
            }

            return user;
        }

        public async Task<ClientToken> Authorize(BaseCredential credentail)
        {
            ApplicationUser user = await userManager.FindByNameAsync(credentail.Username);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid username or passsword");
            }

            var resultSignIn = await signInManager.PasswordSignInAsync(user, credentail.Password, true, true);

            if (!resultSignIn.Succeeded)
            {
                throw new UnauthorizedAccessException("Invalid username or passsword");
            }

            ClientToken token = await GenerateTokenAsync(user);

            return token;
        }

        public async Task<ClientToken> GenerateTokenAsync(ClaimsPrincipal user)
        {
            ApplicationUser currentUser = await userManager.GetUserAsync(user);

            var token = await GenerateTokenAsync(currentUser);

            return token;
        }

        public async Task<ClientToken> GenerateTokenAsync(ApplicationUser user)
        {

            TimeSpan expiredIn = TimeSpan.FromHours(2);

            var acessClaims = await GenerateUserClaimAsync(user);

            var refreshClaims = await GenerateUserClaimAsync(user, isRefreshToken: true);

            string accessToken = GenerateToken(acessClaims, expiredIn);

            string refreshToken = GenerateToken(refreshClaims, TimeSpan.FromDays(14));

            ClientToken token = new ClientToken()
            {
                ExpiredIn = (long)expiredIn.TotalSeconds,
                GrantType = JwtBearerDefaults.AuthenticationScheme,
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            return token;

        }

        protected async Task<IEnumerable<Claim>> GenerateUserClaimAsync(ApplicationUser user, bool isRefreshToken = false)
        {
            List<Claim> claims = new List<Claim>();

            //User Identity claim
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim(ClaimTypes.Sid, user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Authentication, "True"));
            claims.Add(new Claim(ClaimTypes.Actor, "User"));

            if (isRefreshToken) return claims;

            var userRoles = await userManager.GetRolesAsync(user);

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        protected string GenerateToken(IEnumerable<Claim> claims, TimeSpan expiredIn)
        {

            var expiredDate = DateTime.Now.Add(expiredIn);

            var sign = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("Secrets").GetValue<string>("AuthenticationKey"))),
                    SecurityAlgorithms.HmacSha512);

            JwtSecurityToken securityToken = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiredDate,
                signingCredentials: sign);

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            string token = tokenHandler.WriteToken(securityToken);

            return token;
        }

        public Guid GetCurrentUserId()
        {
            var claims = httpContext.HttpContext?.User;

            string? userId = claims?.FindFirstValue(ClaimTypes.Sid);

            if (userId == null) throw new ArgumentNullException("User identity not found.");

            return Guid.Parse(userId);
        }
    }
}
