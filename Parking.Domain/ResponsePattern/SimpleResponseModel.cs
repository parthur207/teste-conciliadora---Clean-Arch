using Parking.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.Domain.ResponsePattern
{
    public class SimpleResponseModel
    {
        public SimpleResponseModel(){}

        public ResponseStatusEnum Status { get; set; }
        public string? Message { get; set; }
    }
}
