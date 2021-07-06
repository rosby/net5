using System.ComponentModel.DataAnnotations;

namespace NsgServerClasses
{
    /// <summary>
    /// Описание полей входа
    /// </summary>
    public class PhoneLoginModel
    {
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string SecurityCode { get; set; }

        /// <summary>
        /// Здесь нужно обязательно вернуть имя пользователя, 
        /// которое потом будет проверяться по аттрибутам авторизации
        /// </summary>
        /// <returns></returns>
        public string GetUserName()
        {
            return PhoneNumber;
        }
        
    }

    public class LoginResponse
    {
        public string Token { get; set; }
        public bool IsAnonymous { get; set; }
        public bool IsError { get; set; }
        public string ErrorMessage { get; set; }
        public int ErrorCode { get; set; }
    }

}
