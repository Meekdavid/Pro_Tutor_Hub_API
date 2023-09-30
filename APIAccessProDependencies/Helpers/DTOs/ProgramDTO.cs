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
    public class ProgramDTO
    {
        [JsonProperty(PropertyName = "id")]
        [Validate]
        public string id { get; set; }
        [Validate]
        public string programId { get; set; }
        [Validate]
        public string programTitle { get; set; }
        [Validate]
        public string programSummary { get; set; }
        [Validate]
        public string programDescription { get; set; }
        [Validate]
        public string[] keySkillsRequired { get; set; }
        [Validate]
        public string programBenefits { get; set; }
        [Validate]
        public string applicationCriteria { get; set; }
        public AdditionalProgramInfo[] additionalProgramInfo { get; set; }
    }
    public class AdditionalProgramInfo
    {
        public string[] programType { get; set; }
        public DateTime programStart { get; set; }
        public DateTime applicationOpen { get; set; }
        public DateTime applicationClose { get; set; }
        [Validate]
        public string duration { get; set; }
        [Validate]
        public string programLocation { get; set; }
        [Validate]
        public string minimumQualification { get; set; }
        [Validate]
        public string maximumNumberOfApplication { get; set; }
    }
}
