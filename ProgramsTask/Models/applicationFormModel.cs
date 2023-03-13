using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramsTask.Models
{
    public class applicationFormModel
    {
        public string? id { get; set; }
        public string? userID { get; set; }
        public string? cover_Image { get; set; }
        public PersonalInfo? personal_Information { get; set; }
        public Profile? Profile { get; set; }
        public AdditionalQuestions? Additional_Questions { get; set; }
    }

    public class PersonalInfo
    {
        public string? First_Name { get; set; }
        public string? Last_Name { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Nationality { get; set; }
        public string? Current_Residence { get; set; }
        public string? ID_Number { get; set; }
        public string? Date_of_Birth { get; set; }
        public string? Gender { get; set; }
        public AddQuestion? Add_a_Question { get; set; }
    }

    public class AddQuestion
    {
        public string? Type { get; set; }
        public string? Question { get; set; }
        public string? Choice { get; set; }
    }

    public class Profile
    {
        public string? Education { get; set; }
        public string? Experience { get; set; }
        public string? resume { get; set; }
    }

    public class AdditionalQuestions
    {
        public string? About_Self { get; set; }
        public string? Select_year_of_graduation { get; set; }
        public AddQuestion? Add_a_Question { get; set; }
        public string? rection_from_US_Embassy { get; set; }
        public AddQuestion? Add_another_Question { get; set; }
    }
}