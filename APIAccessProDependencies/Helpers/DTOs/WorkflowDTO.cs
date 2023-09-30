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
    public class WorkflowDTO
    {
        [JsonProperty(PropertyName = "id")]
        [Validate]
        public string id { get; set; }
        [Validate]
        public string? userID { get; set; }
        [Validate]
        public string programId { get; set; }
        public Stages[]? stages { get; set; }
    }

    public class WorkflowDTOHolder
    {
        [JsonProperty(PropertyName = "id")]
        [Validate]
        public string id { get; set; }
        [Validate]
        public string? userID { get; set; }
        [Validate]
        public string programId { get; set; }
        public string command { get; set; }
        public Stages[]? stages { get; set; }
    }

    public class Stages
    {
        [Validate]
        public string stageName { get; set; }
        [Validate]
        public string stageType { get; set; }
        [Validate]
        public string stageId { get; set; }
        public object stageProps { get; set; }
    }

    public class StageType
    {
        public Applied? applied { get; set; }
        public Shortlisted? shortlisted { get; set; }
        public video_Interview? video_Interview { get; set; }
    }

    public class Applied
    {
        [Validate]
        public string? id { get; set; }
        [Validate]
        public string? userID { get; set; }
        [Validate]
        public string? program_Title { get; set; }
        [Validate]
        public string? program_Summary { get; set; }
        [Validate]
        public string? program_Description { get; set; }
        public string[]? key_Skills_Required { get; set; }
        [Validate]
        public string? program_Benefits { get; set; }
        [Validate]
        public string? application_Criteria { get; set; }
        public Additional_Program_Info? additional_Program_Info { get; set; }
    }

    public class Shortlisted
    {
        [Validate]
        public string? id { get; set; }
        [Validate]
        public string? userID { get; set; }
        [Validate]
        public string? program_Title { get; set; }
        [Validate]
        public string? program_Summary { get; set; }
        [Validate]
        public string? program_Description { get; set; }
        public string[]? key_Skills_Required { get; set; }
        [Validate]
        public string? program_Benefits { get; set; }
        [Validate]
        public string? application_Criteria { get; set; }
        public Additional_Program_Info? additional_Program_Info { get; set; }
    }

    public class Additional_Program_Info
    {
        public string[]? program_Type { get; set; }
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

    public class video_Interview
    {
        [Validate]
        public string? video { get; set; }
        [Validate]
        public string? video_Interview_Question { get; set; }
        [Validate]
        public string? maximum_Duration { get; set; }
        [Validate]
        public string? deadline { get; set; }
    }
}
