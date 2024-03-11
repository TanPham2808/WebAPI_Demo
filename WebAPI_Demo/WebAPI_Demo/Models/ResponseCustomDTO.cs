using Azure;
using System.Net;
using WebAPI_Demo.Constants;

namespace WebAPI_Demo.Models
{
    public class ResponseCustomDTO
    {
        public HttpStatusCodes Code { get; set; } = HttpStatusCodes.Ok;

        public string Message { get; set; }

        public object Data { get; set; }

        public DateTime Time { get; set; } = DateTime.Now;

        public string Status { get; set; } = BaseConstants.Success;

        public static ResponseCustomDTO BadRequest(string errorMessage) => new ResponseCustomDTO
        {
            Code = HttpStatusCodes.BadRequest,
            Message = errorMessage,
            Data = false,
            Status = BaseConstants.Error,
        };

        public static ResponseCustomDTO Ok(object data, string message = null) => new ResponseCustomDTO
        {
            Code = HttpStatusCodes.Ok,
            Message = string.IsNullOrEmpty(message) ? "Success" : message,
            Data = data,
            Status = BaseConstants.Success
        };

    }

}
