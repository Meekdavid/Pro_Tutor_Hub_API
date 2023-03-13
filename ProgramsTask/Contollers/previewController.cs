using Microsoft.AspNetCore.Mvc;
using ProgramsTask.Interfaces;
using ProgramsTask.Processes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgramsTask.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace ProgramsTask.Contollers
{
    [ApiController]
    [Route("[controller]")]
    public class previewController : ControllerBase
    {
        public readonly IPreview _previewService;
        private readonly consoleFeedbackHandler _feedbackService;
        private readonly ILogger<previewController> _logger;
        public previewController(IPreview previewService, consoleFeedbackHandler feedbackService, ILogger<previewController> logger)
        {
            _previewService = previewService;
            _feedbackService = feedbackService;
            _logger = logger;
        }

        [ProducesResponseType(typeof(communicationModels.PreviewAPIResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(communicationModels.NotSuccessfulResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(communicationModels.NotSuccessfulResponse), StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [HttpGet]
        public async void applicationPreview()
        {
            string thisMethod = "Preview Applications";
            var sqlCosmosQuery = "Select * from c";

            var result = await _previewService.applicationPreview(sqlCosmosQuery);
            _feedbackService.previewfeedback(thisMethod, result);
        }

        [ProducesResponseType(typeof(communicationModels.PreviewAPIResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(communicationModels.NotSuccessfulResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(communicationModels.NotSuccessfulResponse), StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [HttpGet]
        [Route("Single Preview")]
        public async void applicationPreviewByUserID()
        {
            Console.WriteLine("\r\n\r\nKindly Input the Required User ID\r\n");

            string userID = Console.ReadLine();
            string thisMethod = "Preview Single Applications";
            var sqlCosmosQuery = $"Select * from c WHERE c.userID = \"{userID}\"";


            var result = await _previewService.applicationPreviewByUserID(sqlCosmosQuery);
            _feedbackService.previewfeedback(thisMethod, result);
        }
    }
}
