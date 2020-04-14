using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace VGT.Server.CustomExceptions
{
    public class HttpResponseException : Exception
    {
        public HttpResponseException(HttpStatusCode Status, object Value)
        {
            this.Status = Status;
            this.Value = Value;
        }
        public HttpStatusCode Status { get; set; } = HttpStatusCode.InternalServerError;

        public object Value { get; set; }
    }
}
