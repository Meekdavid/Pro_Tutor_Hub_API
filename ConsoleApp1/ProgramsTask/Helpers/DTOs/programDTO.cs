using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramsTask.Helpers
{
    public class programDTO
    {
        public string? Id { get; set; }
        public string? UserID { get; set; }
        public string? ProgramTitle { get; set; }
        public string? ProgramSummary { get; set; }
        public string? ProgramDescription { get; set; }
        public string[]? KeySkillsRequired { get; set; }
        public string? ProgramBenefits { get; set; }
        public string? ApplicationCriteria { get; set; }
        public AdditionalProgramInfo[]? AdditionalProgramInfo { get; set; }
    }
    public class AdditionalProgramInfo
    {
        public string[]? ProgramType { get; set; }
        public DateTime? ProgramStart { get; set; }
        public DateTime? ApplicationOpen { get; set; }
        public DateTime? ApplicationClose { get; set; }
        public string? Duration { get; set; }
        public string? ProgramLocation { get; set; }
        public string? MinimumQualification { get; set; }
        public string? MaximumNumberOfApplication { get; set; }
    }
}
