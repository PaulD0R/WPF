using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace WPFTest.Exeptions
{
    public class ApiExeption : Exception
    {

        public ApiExeption(string error = "Ошибка") : base(error)
        {
        }

        public ApiExeption(HttpStatusCode error) : base(GetErrorMessage(error))
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
