using System.Net;

namespace PharmaPro.Core.Contract.Api
{
    public class BaseResponse<T>
    {
        public T? Data { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }

    public class APIResponse<T> : BaseResponse<T>
    {
        public int StatusCode
        {
            get
            {
                return (int)HttpStatusCode;
            }
        }

        public HttpStatusCode HttpStatusCode { get; set; } = HttpStatusCode.OK;

        public bool IsSuccessStatusCode
        {
            get
            {
                return StatusCode >= 200 && StatusCode <= 299;
            }
        }

        public string Message { get; set; }

        public static APIResponse<T> GetNotFoundApiResponse(List<string>? errors = null)
        {
            if (errors is null)
                return new APIResponse<T>()
                {
                    HttpStatusCode = HttpStatusCode.NotFound,
                };

            return new APIResponse<T>()
            {
                HttpStatusCode = HttpStatusCode.NotFound,
                Errors = errors.Any() ? errors : new List<string>()
            };
        }

        public static APIResponse<T> GetBadRequestApiResponse(List<string>? errors = null)
        {
            if (errors is null)
                return new APIResponse<T>()
                {
                    HttpStatusCode = HttpStatusCode.BadRequest,
                };

            return new APIResponse<T>()
            {
                HttpStatusCode = HttpStatusCode.BadRequest,
                Errors = errors.Any() ? errors : new List<string>()
            };
        }
    }
}
