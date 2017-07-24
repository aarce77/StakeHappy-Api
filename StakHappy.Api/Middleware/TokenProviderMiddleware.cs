using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using StakHappy.Api.Data;
using StakHappy.Api.Middleware.DataModels;
using StakHappy.Api.Models;

namespace StakHappy.Api.Middleware
{
    public class TokenProviderMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly TokenProviderOptions _options;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Core.Logic.UserLogic _userLogic;
        private static ApplicationDbContext db = new ApplicationDbContext();
        public TokenProviderMiddleware(
            RequestDelegate next,
            IOptions<TokenProviderOptions> options,
            UserManager<ApplicationUser> userManager,
            Core.Logic.UserLogic userLogic)
        {
            _next = next;
            _options = options.Value;
            _userManager = userManager;
            _userLogic = userLogic;
        }
        public Task Invoke(HttpContext context)
        {
            if (!context.Request.Path.Equals(_options.Path, StringComparison.Ordinal))
            {
                return _next(context);
            }
            if (!context.Request.Method.Equals("POST") || !context.Request.HasFormContentType)
            {
                context.Response.StatusCode = 400;
                return context.Response.WriteAsync("Bad Request");
            }
            return GenerateToken(context);
        }

        private async Task GenerateToken(HttpContext context)
        {
            var username = context.Request.Form["username"].FirstOrDefault();
            var password = context.Request.Form["password"].FirstOrDefault();

            ApplicationUser user = null;
            user = db.Users.Where(x => x.Email == username).FirstOrDefault();

            var result = _userManager.CheckPasswordAsync(user, password);
            if (result.Result == false)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Invalid username or password");
                return;
            }
            var now = DateTime.UtcNow;
            var stakUser = _userLogic.GetByUsername(username);

            var userClaims = await _userManager.GetRolesAsync(user);
            List<Claim> claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new Claim("user_id", stakUser.Id.ToString())
            };

            foreach (var x in userClaims)
                claims.Add(new Claim(ClaimTypes.Role, x));


            var jwt = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(_options.Expiration),
                signingCredentials: _options.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                expires_in = (int)_options.Expiration.TotalSeconds,
                user_id = stakUser.Id
            };

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
        }

    }
}