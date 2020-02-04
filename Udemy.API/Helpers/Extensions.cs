using Microsoft.AspNetCore.Http;

namespace Udemy.API.Helpers
{
    public static class Extensions
    {
        private const string ApplicationError = "Application-Error";
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add(ApplicationError, message);
            response.Headers.Add("Access-Control-Expose-Headers", ApplicationError);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }
    }
}