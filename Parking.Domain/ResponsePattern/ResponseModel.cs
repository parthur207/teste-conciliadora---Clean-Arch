using Parking.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Domain.ResponsePattern
{
    public class ResponseModel<T> 
    {
        public ResponseModel(){}

        public ResponseModel(T? content, string? message, ResponseStatusEnum status)
        {
            Content = content;
            Message = message;
            Status = status;
        }

        public T? Content { get; set; }
        public string? Message { get; set; }
        public ResponseStatusEnum Status { get; set; }
    }
}
