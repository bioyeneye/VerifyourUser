using System;
using System.Collections.Generic;
using System.Text;

namespace VerifyourUser.Domain.ResponseModel
{
    public class GenericResponseModel<T>
    {
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public T Data { get; set; }
    }

    public enum GenericResponseModelStatus
    {
        Success = 0,
        Failed
    }
}
