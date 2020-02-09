using Microsoft.AspNetCore.Http;
using System;

namespace ASPNetCore_Angular.API.Helpers
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

        public static int CalculateAge(this DateTime date)
        {
            var age = DateTime.Now.Year - date.Year;
            if (date.AddYears(age) > DateTime.Today)
                age--;

            return age;
        }
    }
}