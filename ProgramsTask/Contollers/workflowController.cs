using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgramsTask.Interfaces;
using ProgramsTask.Processes;
using ProgramsTask.Helpers;
using Microsoft.AspNetCore.Http;
using ProgramsTask.Models;
using Microsoft.Extensions.Logging;

namespace ProgramsTask.Contollers
{
    [ApiController]
    [Route("[controller]")]
    public class workflowController : ControllerBase
    {
        public readonly IWorkflow _workflowService;
        private readonly consoleFeedbackHandler _feedbackService;
        private readonly ILogger<workflowController> _logger;
        public workflowController(IWorkflow workflowService, consoleFeedbackHandler feedbackService, ILogger<workflowController> logger)
        {
            _workflowService = workflowService;
            _feedbackService = feedbackService;
            _logger = logger;
        }

        [ProducesResponseType(typeof(communicationModels.WorkflowAPIResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(communicationModels.NotSuccessfulResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(communicationModels.NotSuccessfulResponse), StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [HttpGet]
        public async void retrieveFlows()
        {
            string thisMethod = "Retrieve Work Flows";
            var sqlCosmosQuery = "Select * from c";

            var result = await _workflowService.retrieveWorkflows(sqlCosmosQuery);
            _feedbackService.workflowfeedback(thisMethod, result);
        }


        [ProducesResponseType(typeof(communicationModels.WorkflowAPIResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(communicationModels.NotSuccessfulResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(communicationModels.NotSuccessfulResponse), StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [HttpGet]
        [Route("Single Flow")]
        public async void retrieveFlowsbyUserID()
        {
            Console.WriteLine("\r\n\r\nKindly Input the Required User ID\r\n");

            string userID = Console.ReadLine();
            string thisMethod = "Retrieve Work Flows for Specific User";
            var sqlCosmosQuery = $"Select * from c WHERE c.userID = \"{userID}\"";

            var result = await _workflowService.retrieveWorkflowsByUserID(sqlCosmosQuery);
            _feedbackService.workflowfeedback(thisMethod, result);
        }


        [ProducesResponseType(typeof(communicationModels.WorkflowAPIResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(communicationModels.NotSuccessfulResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(communicationModels.NotSuccessfulResponse), StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [HttpPut]
        public async void updateWorkflow()
        {
            string thisMethod = "programUpdate";
            var newToUpdate = new workflowDTO();

            var result = await _workflowService.Update(newToUpdate);
            _feedbackService.updateWorkflowfeedback(thisMethod, result);
        }
    }
}
