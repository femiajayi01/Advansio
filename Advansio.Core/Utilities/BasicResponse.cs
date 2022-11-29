using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advansio.Core.Utilities
{
    public class BasicResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public static BasicResponse SuccessResponse(string message, object data = null) => new BasicResponse { Success = true, Message = message, Data = data };
        public static BasicResponse FailureResponse(string message, string data = null) => new BasicResponse { Success = false, Message = message, Data = data };
        public static BasicResponse FailureResponseWithObject(string message, object data = null) => new BasicResponse { Success = false, Message = message, Data = data };
    }

}
