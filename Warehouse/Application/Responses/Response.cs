using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Responses
{
    public class Response<T> where T : class
    {
        public bool Success { get; set; } = true;
        public string ErrorMessage { get; set; } = string.Empty;
        public T Data { get; set; }
        public int StatusCode { get; set; } = 200;

        public static Response<T> SuccessResponse(T data, int statusCode = 200)
        {
            return new Response<T> { Data = data, StatusCode = statusCode };
        }

        public static Response<T> Fail(string error, int statusCode = 400)
        {
            return new Response<T> { Success = false, ErrorMessage = error, StatusCode = statusCode };
        }
    }
}
