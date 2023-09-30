using APIAccessProDependencies.Helpers.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIAccessProDependencies.Helpers.DTOs.Models
{
    public class ModelIDs
    {
        //[Validate]
        public string documentID { get; set; }
        //[Validate]
        public string programID { get; set; }
        public string? command { get; set; }
    }
}
