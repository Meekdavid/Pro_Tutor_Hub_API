using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramsTask.Models
{
    public class workflowModel
    {
        public string id { get; set; }
        public string userID { get; set; }
        public Stages? Stages { get; set; }
    }

    public class Stages
    {
        public Applied? Applied { get; set; }
        public Shortlisted? Shortlisted { get; set; }
        public video_Interview? video_Interview { get; set; }
        public string? Placement { get; set; }
    }

    public class Applied
    {
        public string? id { get; set; }
        public string? userID { get; set; }
        public string? Program_Title { get; set; }
        public string? Program_Summary { get; set; }
        public string? Program_Description { get; set; }
        public string[]? Key_Skills_Required { get; set; }
        public string? Program_Benefits { get; set; }
        public string? Application_Criteria { get; set; }
        public Additional_Program_Info? Additional_Program_Info { get; set; }
    }

    public class Shortlisted
    {
        public string? id { get; set; }
        public string? userID { get; set; }
        public string? Program_Title { get; set; }
        public string? Program_Summary { get; set; }
        public string? Program_Description { get; set; }
        public string[]? Key_Skills_Required { get; set; }
        public string? Program_Benefits { get; set; }
        public string? Application_Criteria { get; set; }
        public Additional_Program_Info? Additional_Program_Info { get; set; }
    }

    public class Additional_Program_Info
    {
        public string[]? Program_Type { get; set; }
        public string? Program_Start { get; set; }
        public string? Application_Open { get; set; }
        public string? Application_Close { get; set; }
        public string? Duration { get; set; }
        public string? Program_Location { get; set; }
        public string? Minimum_Qualification { get; set; }
        public string? Maximum_Number_of_Application { get; set; }
    }

    public class video_Interview
    {
        public string? video { get; set; }
        public string? video_Interview_Question { get; set; }
        public string? Maximum_Duration { get; set; }
        public string? Deadline { get; set; }
    }
}
