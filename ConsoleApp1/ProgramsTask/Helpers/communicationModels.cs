using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgramsTask.Models;

namespace ProgramsTask.Helpers
{
    public class communicationModels
    {
        public class APIResponse
        {
            public int responseCode { get; set; }
            public string responseMessage { get; set; }
            public string responseMethod { get; set; }
            public List<applicationFormDTO> objectValue { get; set; }
        }

        public class ProgramAPIResponse
        {
            public int responseCode { get; set; }
            public string responseMessage { get; set; }
            public string responseMethod { get; set; }
            public List<programDTO> objectValue { get; set; }
        }

        public class PreviewAPIResponse
        {
            public int responseCode { get; set; }
            public string responseMessage { get; set; }
            public string responseMethod { get; set; }
            public List<previewDTO> objectValue { get; set; }
        }

        public class WorkflowAPIResponse
        {
            public int responseCode { get; set; }
            public string responseMessage { get; set; }
            public string responseMethod { get; set; }
            public List<workflowDTO> objectValue { get; set; }
        }

        public class NotSuccessfulResponse
        {
            public string StatusCode { get; set; }
            public string StatusMessage { get; set; }
        }
    }
}
