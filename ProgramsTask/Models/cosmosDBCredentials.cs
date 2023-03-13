using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgramsTask.Helpers.Config;

namespace ProgramsTask.Models
{

    public static class cosmosDBCredentials
    {
        public static string URI { get; set; } = "https://localhost:8081";
        public static string primaryKey { get; set; } = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        public static string primaryConnectionString { get; set; } = "AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        public static string cosmosConnectionString { get; set; } = "mongodb://localhost:C2y6yDjf5%2FR%2Bob0N8A7Cgv30VRDJIWEHLM%2B4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw%2FJw%3D%3D@localhost:10255/admin?ssl=true";
        public static string cosmosDatabase { get; set; } = "capitalPlacement";
        public static string programContainer { get; set; } = "Program";
        public static string applicationFormContainer { get; set; } = "ApplicationForm";
        public static string workflowContainer { get; set; } = "Workflow";
        public static string previewContainer { get; set; } = "Preview";
    }

}
