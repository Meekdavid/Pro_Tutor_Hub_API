using Microsoft.AspNetCore.Mvc;
using ProgramsTask.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgramsTask.Helpers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ProgramsTask.Processes;
using Microsoft.Extensions.Logging;
//using ProgramsTask.Models;

namespace ProgramsTask.Contollers
{
    [ApiController]
    [Route("[controller]")]
    public class applicationFormContoller : ControllerBase
    {
        public readonly IApplicationForm _applicationFormService;
        private readonly consoleFeedbackHandler _feedbackService;
        private readonly phoneCheck _phoneValidationService;
        private readonly xmlInjectionCheck _xmlInjectionService;
        private readonly ILogger<applicationFormContoller> _logger;
        public applicationFormContoller(IApplicationForm applicationFormService, consoleFeedbackHandler feedbackService, phoneCheck phoneValidationService, xmlInjectionCheck xmlInjectionService,
            ILogger<applicationFormContoller> logger)
        {
            _applicationFormService = applicationFormService;
            _feedbackService = feedbackService;
            _phoneValidationService = phoneValidationService;
            _xmlInjectionService = xmlInjectionService;
            _logger= logger;
        }

        [ProducesResponseType(typeof(communicationModels.APIResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(communicationModels.NotSuccessfulResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(communicationModels.NotSuccessfulResponse), StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [HttpGet]
        public async void retrieveForms()
        {
            string thisMethod = "Retrieve Applicants Forms";
            var sqlCosmosQuery = "Select * from c";

            var result = await _applicationFormService.retrieveForms(sqlCosmosQuery);
            _feedbackService.feedback(thisMethod, result);
        }


        [ProducesResponseType(typeof(communicationModels.APIResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(communicationModels.NotSuccessfulResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(communicationModels.NotSuccessfulResponse), StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [HttpGet]
        [Route("Single Form")]
        public async void retrieveFormsbyUserID()
        {
            Console.WriteLine("\r\n\r\nKindly Input the Required User ID\r\n");

            string userID = Console.ReadLine();
            string thisMethod = "Retrieve Specific Applicants Forms";
            var sqlCosmosQuery = $"Select * from c WHERE c.userID = \"{userID}\"";

            var result = await _applicationFormService.retrieveForms(sqlCosmosQuery);
            _feedbackService.feedback(thisMethod, result);
        }


        [ProducesResponseType(typeof(communicationModels.APIResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(communicationModels.NotSuccessfulResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(communicationModels.NotSuccessfulResponse), StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [HttpPut]
        public async void updateForms()
        {
            string thisMethod = "programUpdate";
            var newToUpdate = new applicationFormDTO();

            var result = await _applicationFormService.Update(newToUpdate);
            _feedbackService.formFeedback(thisMethod, result);
        }

        [ProducesResponseType(typeof(communicationModels.APIResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(communicationModels.NotSuccessfulResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(communicationModels.NotSuccessfulResponse), StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [HttpPost]
        [Route("Delete")]
        public async void deleteForms()
        {
            string thisMethod = "programUpdate";
            Console.WriteLine("Kindly Input the ID required");
            string id = Console.ReadLine();

            Console.WriteLine("Kindly Input the userID required");
            string userID = Console.ReadLine();


            var result = _applicationFormService.Delete(id, userID);
            if (!(result == null))
            {
                Console.WriteLine(JsonConvert.SerializeObject((new communicationModels.APIResponse
                {
                    responseCode = 200,
                    responseMessage = "Success",
                    responseMethod = thisMethod,
                    objectValue = null

                })));
            }

            else
            {
                Console.WriteLine(JsonConvert.SerializeObject((new communicationModels.NotSuccessfulResponse
                {
                    StatusCode = "500",
                    StatusMessage = "An Error Occured, Please Try Again"
                })));
            }
        }

        [ProducesResponseType(typeof(communicationModels.APIResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(communicationModels.NotSuccessfulResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(communicationModels.NotSuccessfulResponse), StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [HttpGet]
        [Route("Delete Specific Arrays")]
        public async void deleteAddedQuestions()
        {
            string thisMethod = "deleteAddedQuestions";
            Console.WriteLine("Kindly Input the Array You are Touching");
            string arrayToDelete = Console.ReadLine();

            Console.WriteLine("Kindly Input the ID of the pecific question you wish to delete");
            string documentID = Console.ReadLine();

            var sqlCosmosQuery = $"UPDATE c SET c.Add_a_Question= ARRAY_REMOVE(c.Add_a_Question, {arrayToDelete}) WHERE c.id = {documentID}";

            var result = await _applicationFormService.retrieveForms(sqlCosmosQuery);
            _feedbackService.feedback(thisMethod, result);
           
        }
    }
}
