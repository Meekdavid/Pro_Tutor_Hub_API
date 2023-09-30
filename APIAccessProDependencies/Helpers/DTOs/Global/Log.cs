using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIAccessProDependencies.Helpers.DTOs.Global
{
    public class Log
    {
        public string MessageLog { get; set; }
        public int LogType { get; set; }
        public Exception ExceptionLog { get; set; } = null;
    }
}
