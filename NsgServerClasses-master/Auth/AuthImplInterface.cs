using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace NsgServerClasses
{
    public interface AuthImplInterface
    {
        /// <summary>
        /// Проверка пользователя
        /// </summary>
        Task<LoginResponse> PhoneLogin(string token, PhoneLoginModel model);
        /// <summary>
        /// Получить новый токен для анонимного пользователя или прошедшего проверку
        /// </summary>
        /// <returns></returns>
        Task<INsgTokenExtension> GetAnonymousToken(INsgTokenExtension tokenExtension);
        /// <summary>
        /// Проверить токен
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<bool> Validate(HttpRequest request, string token);

        Task<INsgTokenExtension> CheckUserToken(string token);
        Task<INsgTokenExtension> GetUserByToken(HttpRequest request);

        /// <summary>
        /// Получить статус по 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        // MyToken GetTokenByUserName(string userName);
        /// <summary>
        /// Получить капчу для дальнейшей авторизации по СМС или другим способом
        /// </summary>
        /// <returns></returns>
        Task<FileStreamResult> GetCapture(string token);
        Task<LoginResponse> PhoneLoginRequestSMS(string accessToken, PhoneLoginModel model);
        Task Logout(string accessToken);
    }

}
