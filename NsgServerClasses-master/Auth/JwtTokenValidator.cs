using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NsgServerClasses.Auth
{
    public class JwtTokenValidator : ISecurityTokenValidator
    {
        static String SecretPhrase = "pole blotting liar absent slater calcite";
        public bool CanReadToken(string securityToken) => true;
        public Func<String, bool> verificateToken;

        public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenValidationParameters = new TokenValidationParameters
            {
                RequireSignedTokens = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "http://nsgsoft.ru",
                ClockSkew = TimeSpan.Zero,
                //ValidAudience = "your string",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretPhrase))
            };

            var claimsPrincipal = handler.ValidateToken(securityToken, tokenValidationParameters, out validatedToken);
            if (verificateToken != null) verificateToken(securityToken);
            return claimsPrincipal;
        }

        //ClaimsPrincipal ISecurityTokenValidator.ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        //{
        //    throw new NotImplementedException();
        //}

        public bool CanValidateToken { get; } = true;
        public int MaximumTokenSizeInBytes { get; set; } = 2048;
    }
}
