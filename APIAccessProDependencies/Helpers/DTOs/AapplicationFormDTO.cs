using APIAccessProDependencies.Helpers.Attributes;
using APIAccessProDependencies.Helpers.DTOs.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIAccessProDependencies.Helpers.DTOs
{
    public class ApplicationFormDTO
    {
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }
        public string? userID { get; set; }
        public string programId { get; set; }
        public string? cover_Image { get; set; }
        public PersonalInfo? personal_Information { get; set; }
        public Profile? profile { get; set; }
        public AdditionalQuestions? additional_Questions { get; set; }
    }

    public class PersonalInfo
    {
        [Validate]
        public string? first_Name { get; set; }
        [Validate]
        public string? last_Name { get; set; }
        [EmailAddress]
        public string? email { get; set; }
        [Validate]
        public string? phone { get; set; }
        [Validate]
        public string? nationality { get; set; }
        [Validate]
        public string? current_Residence { get; set; }
        [Validate]
        public string? iD_Number { get; set; }
        [Validate]
        public string? date_of_Birth { get; set; }
        [Validate]
        public string? gender { get; set; }
        public AddQuestionModel[]? add_a_Question { get; set; }
    }

    public class AddQuestion
    {
        //[Validate]
        public string? type { get; set; }
        //[Validate]
        public string? question { get; set; }
        //[Validate]
        public string? choice { get; set; }
        public ModelIDs ids { get; set; }
    }
    public class AddQuestionModel
    {
        //[Validate]
        public string? type { get; set; }
        //[Validate]
        public string? question { get; set; }
        //[Validate]
        public string? choice { get; set; }
    }
    public class Profile
    {
        [Validate]
        public string? education { get; set; }
        [Validate]
        public string? experience { get; set; }
        [Validate]
        public string? resume { get; set; }
    }

    public class AdditionalQuestions
    {
        [Validate]
        public string? about_Self { get; set; }
        [Validate]
        public string? select_year_of_graduation { get; set; }
        //public AddQuestionModel? add_a_Question { get; set; }
        [Validate]
        public string? rection_from_US_Embassy { get; set; }
        public AddQuestionModel? add_another_Question { get; set; }
    }
}