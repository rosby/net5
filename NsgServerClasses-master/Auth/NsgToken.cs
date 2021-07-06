using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using NsgServerClasses.Auth;

namespace NsgServerClasses
{
    /// <summary>
    /// Класс для работы с токенами
    /// </summary>
    public class NsgToken
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="model"></param>
        public NsgToken(INsgTokenExtension userExtension)
        {
            UserName = userExtension.UserName;
            Role = userExtension.Role;

            ValidFrom = DateTime.UtcNow;
            ValidTo = ValidFrom.AddMonths(1);

            var token = generateJwtToken(userExtension.UserId.ToString(), userExtension.Role);

            AuthId = token;
        }

        static String SecretPhrase = "pole blotting liar absent slater calcite";

        public string generateJwtToken(String userId, String userRole)
        {

            //Set issued at date
            DateTime issuedAt = ValidFrom;
            //set the time when it expires
            DateTime expires = ValidTo;

            //http://stackoverflow.com/questions/18223868/how-to-encrypt-jwt-security-token
            var tokenHandler = new JwtSecurityTokenHandler();

            //create a identity and add claims to the user which we want to log in
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim("id", userId),
                new Claim("role", userRole)
        });

            var now = DateTime.UtcNow;
            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(SecretPhrase));
            var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature);


            //create the jwt
            var token =
                (JwtSecurityToken)
                    tokenHandler.CreateJwtSecurityToken(issuer: "http://nsgsoft.ru", audience: "http://nsgsoft.ru",
                        subject: claimsIdentity, notBefore: issuedAt, expires: expires, signingCredentials: signingCredentials);
            var tokenString = tokenHandler.WriteToken(token);

            return "Bearer " + tokenString;

        }

       /* private String generateJwtToken(String userId, String userRole)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(SecretPhrase);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", userId), new Claim("role", userRole) }),
                Expires = ValidTo,
                Issuer = "http://nsgsoft.ru",
                SigningCredentials = new SigningCredentials(new System.IdentityModel.InMemorySymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature, "")
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }*/

        static public bool ValidateToken(string token)
        {
            try
            {
                if (token.StartsWith("Bearer ")) token = token.Split(' ')[1];
                var tokenHandler = new JwtTokenValidator();
                if (tokenHandler.ValidateToken(token, null, out SecurityToken validatedToken) != null)
                {

                    var jwtToken = (JwtSecurityToken)validatedToken;
                    var userId = (jwtToken.Claims.First(x => x.Type == "id").Value);

                    return true;
                }
                else
                {
                    Console.WriteLine($"{DateTime.Now} - ValidateToken - {token}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                //validation failed, do nothing
                Console.WriteLine($"{DateTime.Now} - ValidateToken - {ex.Message}");
                return false;
            }
        }

        static private void issureValidator(SecurityKey key)
        {

        }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string UserName
        {
            get;
            private set;
        }
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string Role
        {
            get;
            private set;
        }
        /// <summary>
        /// Ключ для клиента
        /// </summary>
        public string AuthId { get; private set; }
        /// <summary>
        /// Время начала действия
        /// </summary>
        public DateTime ValidFrom { get; set; }
        /// <summary>
        /// Время окончания действия
        /// </summary>
        public DateTime ValidTo { get; set; }
        
    }

    public interface INsgTokenExtension
    {
        string Token { get; set; }
        Guid UserId { get; set; }
        string Phone { get; set; }
        string UserName { get; set; }
        string Role { get; set; }

        List<System.Security.Claims.Claim> GetClaimsAndSetPrincipal(HttpRequest request);
        NsgToken CreateNsgToken();

        DateTime LastAccessTime { get; set; }
    }
}
