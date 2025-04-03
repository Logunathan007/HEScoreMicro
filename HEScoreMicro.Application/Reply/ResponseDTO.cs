using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericController.Application.Mapper.Reply
{
    public class Response
    {
        public bool Failed { get; set; }
        public string Message { get; set; }
    }

    public class ResponseDTO<T> : Response
    {
        public T? Data { get; set; }
    }
}
