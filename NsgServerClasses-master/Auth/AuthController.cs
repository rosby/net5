using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace NsgServerClasses
{
    [Route("Api/Auth")]
    [ApiController]
    //[AllowAnonymous]
    public class AuthController : ControllerBase
    {
        public static AuthImplInterface currentController;
        public static AuthImplInterface CurrentController
        {
            get
            {
                if (currentController == null)
                {
                    currentController = new AuthImplMock();
                }
                return currentController;
            }
        }
        static AuthController()
        {
        }

        [Route("AnonymousLogin")]
        [HttpGet]
        public async Task<IActionResult> AnonymousLogin()
        {
            Console.WriteLine("AnonymousLogin");
            INsgTokenExtension tokenExtension = new NsgTokenItem()
            {
                UserId = Guid.NewGuid(),
                Role = UserRoles.Anonymous,
                UserName = UserRoles.Anonymous
            };
            
            //var item = CurrentController.CheckLogin(userSettings);
            var key = await CurrentController.GetAnonymousToken(tokenExtension);
            Console.WriteLine($"AnonymousLogin - new token {key.Token}");
            if (key != null)
                return Ok(new LoginResponse
                {
                    Token = key.Token,
                    IsError = false,
                    ErrorMessage = string.Empty,
                    IsAnonymous = true
                });
            else
                return Ok(new LoginResponse
                {
                    IsError = true,
                    ErrorMessage = $"ERROR"
                });
        }

        [Route("CheckToken")]
        [HttpGet]
        public async Task<IActionResult> CheckToken()
        {
            var accessToken = GetAccessToken(Request);
            int errorCode = 0;
            var user = await CurrentController.CheckUserToken(accessToken);
            if (user != null)
            {
                return Ok(new LoginResponse
                {
                    Token = user.Token,
                    IsError = false,
                    ErrorMessage = string.Empty,
                    ErrorCode = errorCode
                });
            }
            else
                return Ok(new LoginResponse
                {
                    IsError = true,
                    ErrorMessage = $"Wrong code",
                    ErrorCode = 401
                });
        }

        /// <summary> Вход в систему </summary>
        [Route("PhoneLogin")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PhoneLogin([FromBody] PhoneLoginModel model)
        {
            var accessToken = GetAccessToken(Request);
            int errorCode = 0;
            var user = await CurrentController.PhoneLogin(accessToken, model);
            if (user != null)
            {
                return Ok(new LoginResponse
                {
                    Token = user.Token,
                    IsError = user.IsError,
                    ErrorMessage = user.ErrorMessage,
                    ErrorCode = user.ErrorCode
                });
            }
            else
                return Ok(new LoginResponse
                {
                    IsError = true,
                    ErrorMessage = $"Wrong code",
                    ErrorCode = errorCode
                });
        }

        /// <summary> Запрос СМС для авторизации по телефону </summary>
        [Route("PhoneLoginRequestSMS")]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PhoneLoginRequestSMS([FromBody] PhoneLoginModel model)
        {
            var accessToken = GetAccessToken(Request);
            int errorCode = 0;
            var user = await CurrentController.PhoneLoginRequestSMS(accessToken, model);
            if (user != null && !user.IsError)
                return Ok(new LoginResponse
                {
                    Token = user.Token,
                    IsError = false,
                    ErrorMessage = string.Empty,
                    ErrorCode = 0
                }); 
            else
                return Ok(new LoginResponse
                {
                    IsError = true,
                    ErrorMessage = user.ErrorMessage,
                    ErrorCode = user.ErrorCode
                });
        }
        /// <summary> Выход </summary>
        [Route("Logout")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var accessToken = GetAccessToken(Request);
            await CurrentController.Logout(accessToken);
            return Ok();
        }

        /// <summary>
        /// Получить капчу для дальнейшей авторизации по СМС 
        /// </summary>
        /// <returns></returns>
        [Route("GetCaptcha")]
        [HttpGet]
        [Authorize]
        public async Task<FileStreamResult> GetCaptcha()
        {
            var accessToken = GetAccessToken(Request);
            Console.WriteLine($"GetCaptcha - token {accessToken}");
            return await CurrentController.GetCapture(accessToken);
        }

        static public string GetAccessToken(HttpRequest request)
        {
            var values = new StringValues();
            var accessToken = "";
            if (request.Headers.TryGetValue("Authorization", out values))
                accessToken = values.FirstOrDefault();
            return accessToken;
        }

    }
}
