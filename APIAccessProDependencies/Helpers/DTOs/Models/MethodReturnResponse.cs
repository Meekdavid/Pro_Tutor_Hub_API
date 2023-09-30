using APIAccessProDependencies.Helpers.DTOs.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIAccessProDependencies.Helpers.DTOs.Models
{
    public class MethodReturnResponse<T>
    {
        public T? objectValue { get; set; }
        public bool ? success { get; set; }
        public List<Log> Logs { get; set; }
        public string _message { get; set; }
    }
}
