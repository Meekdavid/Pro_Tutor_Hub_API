using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProgramsTask.Helpers;
using ProgramsTask.Interfaces;
using ProgramsTask.Models;
using ProgramsTask.Processes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramsTask.Contollers
{
    [ApiController]
    [Route("[controller]")]
    public class programController : ControllerBase
    {
        public readonly IProgram _programmService;
        private readonly consoleFeedbackHandler _feedbackService;
        private readonly phoneCheck _phoneValidationService;
        private readonly xmlInjectionCheck _xmlInjectionService;
        private readonly ILogger<programController> _logger;
        public programController(IProgram programmService, consoleFeedbackHandler feedbackService, phoneCheck phoneCheck, xmlInjectionCheck xmlInjectionCheck,
            ILogger<programController> logger)
        {
            _programmService = programmService;
            _feedbackService = feedbackService;
            _phoneValidationService = phoneCheck;
            _feedbackService = feedbackService;
            _xmlInjectionService= xmlInjectionCheck;
            _logger = logger;
        }

        [ProducesResponseType(typeof(communicationModels.ProgramAPIResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(communicationModels.NotSuccessfulResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(communicationModels.NotSuccessfulResponse), StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [HttpGet]
        public async void retrievePrograms()
        {
            string thisMethod = "Retrieve Program Details";
            var sqlCosmosQuery = "Select * from c";

            var result = await _programmService.retrievePrograms(sqlCosmosQuery);
            _feedbackService.programfeedback(thisMethod, result);
        }

        [ProducesResponseType(typeof(communicationModels.ProgramAPIResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(communicationModels.NotSuccessfulResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(communicationModels.NotSuccessfulResponse), StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [HttpGet]
        [Route("Single Program")]
        public async void retrieveProgramsByUserID()
        {
            string thisMethod = "Retrieve Program Details";
            Console.WriteLine("\r\n\r\nKindly Input the Required User ID\r\n");

            string userID = Console.ReadLine();
            var sqlCosmosQuery = $"Select * from c WHERE c.userID = \"{userID}\"";

            var result = await _programmService.retrieveProgramsByUserID(sqlCosmosQuery);
            _feedbackService.programfeedback(thisMethod, result);
        }

        [ProducesResponseType(typeof(communicationModels.ProgramAPIResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(communicationModels.NotSuccessfulResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(communicationModels.NotSuccessfulResponse), StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [HttpPost]
        public async void addPrograms()
        {
            string thisMethod = "Add a New Program";
            var newProgramModel = new programDTO();

            newProgramModel.Id = Guid.NewGuid().ToString();

            Console.WriteLine("Kindly Input Your user ID");
            string thisUser = Console.ReadLine().ToString();
            while(_xmlInjectionService.xmlCheck(thisUser).ToUpper() == "FALSE")
            {
                Console.WriteLine("Please Enter a Valid ID");
                thisUser = Console.ReadLine();
            }
            newProgramModel.UserID = thisUser;

            Console.WriteLine("Kindly Input Program Title");
            newProgramModel.ProgramTitle = Console.ReadLine();

            Console.WriteLine("Kindly Input Program Summary");
            newProgramModel.ProgramSummary = Console.ReadLine();

            Console.WriteLine("Kindly Input Program Description");
            newProgramModel.ProgramDescription = Console.ReadLine();

            string[] inputs = new string[7];
            // loop 10 times to read inputs from the console and add them to the array
            for (int i = 0; i <= inputs.Length-2; i++)
            {
                Console.Write("Enter a New Skill #" + (i + 1) + ": ");
                string input = Console.ReadLine();
                inputs[i] = input;
            }
            newProgramModel.KeySkillsRequired = inputs;
           

            Console.WriteLine("Kindly Input Program Benefits");
            newProgramModel.ProgramBenefits = Console.ReadLine();

            Console.WriteLine("Kindly Input Program Acceptance Criteria");
            newProgramModel.ApplicationCriteria = Console.ReadLine();

            string[] programType = new string[] {"Internships", "Webinar", "Job", "Course", "Training", "Live Seminar", "MasterClass", "Volunteering", "Others" };
            Console.WriteLine($"Kindly Select Program Type From the List\r\n {JsonConvert.SerializeObject(programType)}");
            string[] inputsProgram = new string[5];
            // loop 10 times to read inputs from the console and add them to the array
            for (int i = 0; i < inputsProgram.Length-1; i++)
            {
                Console.Write("\r\nEnter a New Program Type #" + (i + 1) + ": ");
                string inputsPrograms = Console.ReadLine();
                inputsProgram[i] = inputsPrograms;
            }
            var chdye = newProgramModel.AdditionalProgramInfo[0].ProgramType;
            newProgramModel.AdditionalProgramInfo[0].ProgramType = inputsProgram;
            Console.WriteLine("Kindly Input Duration Of Program");
            newProgramModel.AdditionalProgramInfo[0].Duration = Console.ReadLine();

            Console.WriteLine("Kindly Enter Program Start Date");
            newProgramModel.AdditionalProgramInfo[0].ProgramStart = DateTime.Parse(Console.ReadLine());

            Console.WriteLine("Kindly Input Program Open Date");
            newProgramModel.AdditionalProgramInfo[0].ApplicationOpen = DateTime.Parse(Console.ReadLine());

            Console.WriteLine("Kindly Input Program Close Date");
            newProgramModel.AdditionalProgramInfo[0].ApplicationClose = DateTime.Parse(Console.ReadLine());

            Console.WriteLine("Kindly Input Location Of Program");
            newProgramModel.AdditionalProgramInfo[0].ProgramLocation = Console.ReadLine();

            Console.WriteLine("Kindly Input Mininumum Qualification for Program");
            newProgramModel.AdditionalProgramInfo[0].MinimumQualification = Console.ReadLine();

            Console.WriteLine("Kindly Input the Maximum Number of Qualification");
            newProgramModel.AdditionalProgramInfo[0].MaximumNumberOfApplication = Console.ReadLine();


            var result = await _programmService.AddProgramAsync(newProgramModel);
            _feedbackService.addProgramfeedback(thisMethod, result);
        }

        [ProducesResponseType(typeof(communicationModels.ProgramAPIResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(communicationModels.NotSuccessfulResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(communicationModels.NotSuccessfulResponse), StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [HttpPut]
        public async void updateProgram()
        {
            string thisMethod = "programUpdate";
            var newToUpdate = new programDTO();



            var result = await _programmService.Update(newToUpdate);
            _feedbackService.addProgramfeedback(thisMethod, result);
        }
    }
}
