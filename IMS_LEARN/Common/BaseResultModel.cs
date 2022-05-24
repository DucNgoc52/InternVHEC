using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMS_LEARN.Common
{
    public class BaseResultModel
    {
        public int Code { get; set; } = 0;
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = "";
        public object Data { get; set; }
    }
}
