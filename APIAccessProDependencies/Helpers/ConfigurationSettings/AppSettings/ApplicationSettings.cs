using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIAccessProDependencies.Helpers.ConfigurationSettings.AppSettings
{
    public class ApplicationSettings
    {
        public string URI { get; set; }
        public string primaryKey { get; set; }
        public string cosmosDatabase { get; set; }
        public string programContainer { get; set; }
        public string applicationFormContainer { get; set; }
        public string workflowContainer { get; set; }
        public string previewContainer { get; set; }
        public int OtherServicesCacheTime { get; set; }
        public bool ActivateResponseCaching { get; set; }
    }
}
