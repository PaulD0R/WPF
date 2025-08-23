using System.Net;

namespace WPFTest.Exeptions
{
    public class ApiException : Exception
    {

        public ApiException(string error = "Ошибка") : base(error)
        {
        }

        public ApiException(HttpStatusCode error) : base(GetErrorMessage(error))
        {
        }

        private static string GetErrorMessage(HttpStatusCode errorCode)
        {
            return errorCode switch
            {
                HttpStatusCode.NotFound => "Не найдено",
                HttpStatusCode.BadRequest => "Неверный запрос",
                HttpStatusCode.Unauthorized => "Не авторизован",
                HttpStatusCode.Forbidden => "Доступ запрещен",
                _ => "Ошибка"
            };
        }
    }
}
