using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NsgServerClasses;

namespace NsgServerClasses
{
    public class AuthSmsData
    {
        /// <summary>
        /// Сгенерированный проверочный код, отправляемый по СМС 
        /// </summary>
        public string SecurityCode;
        /// <summary>
        /// Номер телефона, на который осуществляется отправка СМС
        /// </summary>
        public string Phone;
        /// <summary>
        /// Время генерации кодя для проверки времени его жизни
        /// </summary>
        public DateTime GenerationTime;
        /// <summary>
        /// Количество попыток неправильного ввода кода, для избежания возможности перебора
        /// </summary>
        public int AttemptsCount;
        
    }

    public class AuthSmsDataController
    {
        /// <summary>
        /// Словарь соответствия токенов и выданных кодов
        /// </summary>
        public ConcurrentDictionary<string, AuthSmsData> smsDictionary = new ConcurrentDictionary<string, AuthSmsData>();
        /// <summary>
        /// Время жизни выданного кода. По истечении указанного времени необходимо получить новый код. По умолчанию - 180 секунд
        /// </summary>
        public int AliveTime = 180;
        /// <summary>
        /// Минимальное интервал времени, после которого возможна повторная отправка СМС 
        /// </summary>
        public int MinRepeateTime = 120;
        /// <summary>
        /// Максимальное количество попыток ввода кода. По умолчнию 3
        /// </summary>
        public int MaxAttemptsCount = 3;
        /// <summary>
        /// Сгенерировать и отправить через смс проверочный код
        /// </summary>
        /// <param name="token">токен пользователя</param>
        /// <param name="phone">телефон пользователя</param>
        /// <returns>код ошибки:
        /// 0 - отправка осуществлена успешно,
        /// 40200 - отправка невозможна, т.к. действует предыдущий отправленный код
        /// 40201 - прочие ошибки при отправке. Возможно, указан неверный номер телефона</returns>
        public int SendSecurityCodeBySms(string token, string phone)
        {
            AuthSmsData oldData = smsDictionary.Values.FirstOrDefault(x => x.Phone == phone);
            if (oldData != null)
            {
                if ((DateTime.Now - oldData.GenerationTime).TotalSeconds < MinRepeateTime)
                        return 40200;
            }
            var smsCode = "999999";
            var data = new AuthSmsData();
            data.SecurityCode = smsCode;
            data.Phone = phone;
            data.GenerationTime = DateTime.Now;
            smsDictionary.TryAdd(token, data);
            //TODO: Сделать генерацию кода и отправку  его через смс
            return 0;
        }
        /// <summary>
        /// Проверка кода безопасности
        /// </summary>
        /// <param name="token">Токен, по которому запрашивался код</param>
        /// <param name="securityCode">Проверяемый код безопасности</param>
        /// <returns>Код ошибки
        /// 0 - проверка завершена успешно, код совпадает
        /// 40300 - код неверный, еще есть попытки для ввода нового
        /// 40301 - код неверный, превышено количество ошибок
        /// 40302 - код устарел
        /// 40303 - не найден код по токену</returns>
        public async Task<int> CheckSecurityCode(string token, string securityCode)
        {
            AuthSmsData data = null;
            if (!smsDictionary.TryGetValue(token, out data))
                return 40303;
            if ((DateTime.Now - data.GenerationTime).TotalSeconds > AliveTime)
            {
                smsDictionary.TryRemove(token, out data);
                return 40302;
            }
            if (data.SecurityCode != securityCode)
            {
                data.AttemptsCount++;
                if (data.AttemptsCount >= MaxAttemptsCount)
                {
                    smsDictionary.TryRemove(token, out data);
                    return 40301;
                }
                return 40300;
            }
            smsDictionary.TryRemove(token, out data);
            return 0;
        }

    }
}
