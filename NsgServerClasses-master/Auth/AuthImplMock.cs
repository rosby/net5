using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NsgServerClasses;

namespace NsgServerClasses
{
    /// <summary>
    /// Реализация проверки пользователя
    /// </summary>
    public class AuthImplMock : AuthImplInterface
    {
        /// <summary>
        /// кэш выданных токенов
        /// </summary>
        static readonly ConcurrentDictionary<string, INsgTokenExtension> tokens = new ConcurrentDictionary<string, INsgTokenExtension>();
        /// <summary>
        /// Отправленные коды авторизации
        /// </summary>
        static AuthSmsDataController smsDataController = new AuthSmsDataController();
        ConcurrentDictionary<string, NsgCaptcha.NsgCaptchaResult> captchaDictionary = new ConcurrentDictionary<string, NsgCaptcha.NsgCaptchaResult>();
        /// <summary>
        /// Создаем супер рута для подключения из вне
        /// </summary>
        static AuthImplMock()
        {
        }
        /// <summary>
        /// Проверка пользователя
        /// </summary>
        public async Task<LoginResponse> PhoneLogin(string token, PhoneLoginModel model)
        {
            int codeError = 0;
            if (model == null || String.IsNullOrEmpty(model.PhoneNumber))
            {
                codeError = 40104; //Необходимо указать номер телефона
                return new LoginResponse
                {
                    IsError = true,
                    ErrorMessage = $"Необходимо указать номер телефона",
                    ErrorCode = codeError
                };
            }
            if (String.IsNullOrEmpty(model.SecurityCode))
            {
                codeError = 40105; //Необходимо указать текст капчи
                return new LoginResponse
                {
                    IsError = true,
                    ErrorMessage = $"Необходимо указать текст капчи",
                    ErrorCode = codeError
                };
            }

            codeError = await smsDataController.CheckSecurityCode(token, model.SecurityCode);
            if (codeError != 0)
                return null;

            //Телефон берем с предыдущего шага, т.к. именно на нем он был верифицирован
            INsgTokenExtension extensions = null;
            if (!tokens.TryGetValue(token, out extensions))
            { 
                codeError = 40106; //Не найден токен. Неизвестная ошибка. Скорее всего, сервер перезапущем с момента выдачи токена или срок жизни токена истек
                return new LoginResponse
                {
                    IsError = true,
                    ErrorMessage = $"Истек срок жижни токена.",
                    ErrorCode = codeError
                };
            }

            extensions.UserId = Guid.NewGuid();
            extensions.Role = UserRoles.User;
            extensions.UserName = "Пользователь";

            var item = extensions.CreateNsgToken();
            extensions.Token = item.AuthId;

            
            return new LoginResponse() {
                Token = extensions.Token,
                IsError = false,
                ErrorMessage = string.Empty,
                ErrorCode = codeError
            };
        }
        /// <summary>
        /// Проверить токен
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<bool> Validate(HttpRequest request, string token)
        {
            var user = await CheckUserToken(token);

            if (user != null)
            {
                user.GetClaimsAndSetPrincipal(request);
                return true;
            }
            return false;
        }

        public async Task<FileStreamResult> GetCapture(string token)
        {
            var captcha = new NsgCaptcha();
            var captchaAnswer = await captcha.GetCaptchaResult();
            captchaDictionary[token] = captchaAnswer;
            var pic = captchaAnswer.Image;
            var stream = new System.IO.MemoryStream();
            pic.Save(stream, ImageFormat.Png);
            stream.Position = 0;

            string file_type = "image/png";
            return new FileStreamResult(stream, file_type) { FileDownloadName = "captcha.png" };
            //HttpResponseMessage responseMsg = new FileStreamResult() new HttpResponseMessage(HttpStatusCode.OK);
            //responseMsg.Content = new StreamContent(stream);
            //responseMsg.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            //return responseMsg;
        }

        public async Task<INsgTokenExtension> CheckUserToken(string token)
        {
            INsgTokenExtension user = null;
            if (NsgToken.ValidateToken(token))
            {
                if (tokens.TryGetValue(token, out user))
                {
                    //UpdateActivity(token);
                    return user;
                }
                var extensions = new NsgTokenItem()
                {
                    Token = token,
                    UserId = Guid.NewGuid(),
                    Role = UserRoles.User,
                    UserName = "Пользователь",
                };
                tokens.TryAdd(extensions.Token, extensions);
                return extensions;
            }
            return null;
        }

        public async Task<INsgTokenExtension> GetAnonymousToken(INsgTokenExtension tokenExtension)
        {
            tokenExtension.Token = tokenExtension.CreateNsgToken().AuthId;
            tokenExtension.Role = UserRoles.Anonymous;
            tokens.TryAdd(tokenExtension.Token, tokenExtension as INsgTokenExtension);
            return tokenExtension;
        }

        public async Task<LoginResponse> PhoneLoginRequestSMS(string token, PhoneLoginModel model)
        {
            int codeError = 0;
            NsgCaptcha.NsgCaptchaResult captcha = null;
            if (model == null || String.IsNullOrEmpty(model.PhoneNumber))
            {
                codeError = 40104; //Необходимо указат номер телефона
                return new LoginResponse
                {
                    IsError = true,
                    ErrorMessage = $"Необходимо указать номер телефона",
                    ErrorCode = codeError
                };
            }
            if (String.IsNullOrEmpty(model.SecurityCode))
            {
                codeError = 40105; //Необходимо указать текст капчи
                return new LoginResponse
                {
                    IsError = true,
                    ErrorMessage = $"Необходимо указать текст капчи",
                    ErrorCode = codeError
                };
            }
            if (!captchaDictionary.TryRemove(token, out captcha))
            {
                codeError = 40101; //Перед вызовом метода PhoneLogin необходимо получить капчу, вызвав метод GetCaptcha
                return new LoginResponse
                {
                    IsError = true,
                    ErrorMessage = $"Необходимо получить новую капчу",
                    ErrorCode = codeError
                };
            }
            if ((DateTime.Now - captcha.CreateDate).TotalSeconds > 120)
            {
                codeError = 40102; //Captcha устарела, необходимо получить новую
                return new LoginResponse
                {
                    IsError = true,
                    ErrorMessage = $"Капча устарела",
                    ErrorCode = codeError
                };
            }
            if (model.SecurityCode.ToLower() != captcha.Text.ToLower())
            {
                codeError = 40103; //Передан неверный текст капчи
                return new LoginResponse
                {
                    IsError = true,
                    ErrorMessage = $"Неверный текст капчи",
                    ErrorCode = codeError
                };
            }

            INsgTokenExtension item = null;
            if (!tokens.TryGetValue(token, out item))
            {
                codeError = 40106; //Не найден токен. Неизвестная ошибка.
                return new LoginResponse
                {
                    IsError = true,
                    ErrorMessage = $"Не найден токен",
                    ErrorCode = codeError
                };
            }
            item.Phone = model.PhoneNumber;
            codeError = smsDataController.SendSecurityCodeBySms(token, model.PhoneNumber);
            if (codeError == 0)
                return new LoginResponse()
                {
                    Token = token,
                    IsError = false,
                    ErrorMessage = string.Empty,
                    ErrorCode = codeError
                };
            else
            {
                codeError = 40201; //Не найден токен. Неизвестная ошибка.
                return new LoginResponse
                {
                    IsError = true,
                    ErrorMessage = $"Ошибка при отправке СМС",
                    ErrorCode = codeError
                };
            }
        }

        public async Task Logout(string token)
        {
            //Удаляем токен из кэша
            INsgTokenExtension user;
            tokens.TryRemove(token, out user);
            //Удаляем токен из базы
        }

        public async Task<INsgTokenExtension> GetUserByToken(HttpRequest request)
        {
            var token = request.Headers["Authorization"].ToString();
            INsgTokenExtension user = null;
            tokens.TryGetValue(token, out user);
            return user;
            //throw new NotImplementedException();
        }
    }
}
