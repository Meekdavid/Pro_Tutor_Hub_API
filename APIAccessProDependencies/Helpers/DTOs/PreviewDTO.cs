using APIAccessProDependencies.Helpers.Attributes;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIAccessProDependencies.Helpers.DTOs
{
    public class PreviewDTO : ProgramDTO
    {
        //[JsonProperty(PropertyName = "id")]
        //[Validate]
        //public string id { get; set; }
        //[Validate]
        //public string? userID { get; set; }
        //[Validate]
        //public string programId { get; set; }
        //public Program? Program { get; set; }
    }

    public class Program
    {
        [Validate]
        public string? program_Title { get; set; }
        [Validate]
        public string? program_Summary { get; set; }
        [Validate]
        public string? program_Description { get; set; }
        public List<string>? key_Skills_Required { get; set; }
        [Validate]
        public string? program_Benefits { get; set; }
        [Validate]
        public string? application_Criteria { get; set; }
        public AdditionalProgramInfon? additional_Program_Info { get; set; }
    }

    public class AdditionalProgramInfon
    {
        public List<string>? program_Type { get; set; }
        [Validate]
        public string? program_Start { get; set; }
        [Validate]
        public string? application_Open { get; set; }
        [Validate]
        public string? application_Close { get; set; }
        [Validate]
        public string? duration { get; set; }
        [Validate]
        public string? program_Location { get; set; }
        [Validate]
        public string? minimum_Qualification { get; set; }
        [Validate]
        public string? maximum_Number_of_Application { get; set; }
    }
}
