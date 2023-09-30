using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramsTask.Models
{
    public class PreviewModel
    {
        public string id { get; set; }
        public string userID { get; set; }
        public Program? Program { get; set; }
    }

    public class Program
    {
        public string? Program_Title { get; set; }
        public string? Program_Summary { get; set; }
        public string? Program_Description { get; set; }
        public List<string>? Key_Skills_Required { get; set; }
        public string? Program_Benefits { get; set; }
        public string? Application_Criteria { get; set; }
        public AdditionalProgramInfon? Additional_Program_Info { get; set; }
    }

    public class AdditionalProgramInfon
    {
        public List<string>? Program_Type { get; set; }
        public string? Program_Start { get; set; }
        public string? Application_Open { get; set; }
        public string? Application_Close { get; set; }
        public string? Duration { get; set; }
        public string? Program_Location { get; set; }
        public string? Minimum_Qualification { get; set; }
        public string? Maximum_Number_of_Application { get; set; }
    }
}
