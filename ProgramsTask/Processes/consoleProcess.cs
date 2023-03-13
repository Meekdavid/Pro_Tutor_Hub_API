using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ProgramsTask.Helpers.Config;
using ProgramsTask.Models;
using ProgramsTask.Interfaces;
using ProgramsTask.Helpers;
using Newtonsoft.Json;
using ProgramsTask.Contollers;
using Microsoft.Extensions.Logging;

namespace ProgramsTask.Processes
{
    [Route("api/[controller]")]
    [ApiController]
    public class consoleProcess : ControllerBase
    {
        public readonly IApplicationForm _applicationFormService;
        private readonly applicationFormContoller _formService;
        private readonly programController _programService;
        private readonly workflowController _workflowController;
        private readonly previewController _previewController;
        private readonly ILogger<consoleProcess> _logger;
        public consoleProcess(IConfiguration configuration, IApplicationForm applicationForm, applicationFormContoller formService,
            programController programService, ILogger<consoleProcess> logger, workflowController workflowController, previewController previewController)
        {
            Configuration = configuration;
            ConfigurationSettingsHelper.Configuration = configuration;
            _applicationFormService = applicationForm;
            _formService = formService;
            _programService = programService;
            _logger = logger;
            _workflowController = workflowController;
            _previewController = previewController;
        }

        public IConfiguration Configuration { get; }
        public void API()
        {
            _logger.LogInformation("About Executing the Entry point to the CRUD APIs");
            Console.WriteLine("Kindly follow instructions below to proceed:\r\n\r\n" +
                "Enter 1 to access the Program Controller\r\n" +
                "Enter 2 to access the Application Form Contoller\r\n" +
                "Enter 3 to access the Workflow Controller\r\n" +
                "Enter 4 to Preview Applications" +
                "Enter 5 to Repeat" +
                "Enter 6 to Exit the Application");

            int userInput = int.Parse(Console.ReadLine());
            if (userInput == 1)
            {
                Console.WriteLine("Kindly follow instructions below to proceed:\r\n\r\n" +
                "Enter 1 to Retreive Programs\r\n" +
                "Enter 2 to Retrieve Programs by User ID\r\n" +
                "Enter 3 to Add Programs\r\n" +
                "Enter 4 to Update Programs" +
                "Enter 5 to Repeat" +
                "Enter 6 to Exit");

                int thisInput = int.Parse(Console.ReadLine());

                if (thisInput == 1)
                {
                    _programService.retrievePrograms();
                }else if(thisInput == 2)
                {
                    _programService.retrieveProgramsByUserID();
                }
                else if (thisInput == 3)
                {
                    _programService.addPrograms();
                }
                else if (thisInput == 4)
                {
                    _programService.updateProgram();
                }else if (thisInput == 5)
                {
                    API();
                }else if (thisInput == 6)
                {
                    Environment.Exit(00001);
                }

            }
            else if(userInput == 2)
            {
                Console.WriteLine("Kindly follow instructions below to proceed:\r\n\r\n" +
                "Enter 1 to Retreive Forms\r\n" +
                "Enter 2 to Retrieve Forms by User ID\r\n" +
                "Enter 3 to Update Forms\r\n" +
                "Enter 4 to Delete Forms" +
                "Enter 5 to Delete Added Questions" +
                "Enter 6 to Repeat" +
                "Enter 7 to Exit");

                int thisInput = int.Parse(Console.ReadLine());

                if (thisInput == 1)
                {
                    _formService.retrieveForms();
                }
                else if (thisInput == 2)
                {
                    _formService.retrieveFormsbyUserID();
                }
                else if (thisInput == 3)
                {
                    _formService.updateForms();
                }
                else if (thisInput == 4)
                {
                    _formService.deleteForms();
                }
                else if (thisInput == 5)
                {
                    _formService.deleteAddedQuestions();
                }
                else if (thisInput == 6)
                {
                    API();
                }
                else if (thisInput == 7)
                {
                    Environment.Exit(00002);
                }
            }
            else if (userInput == 3)
            {
                Console.WriteLine("Kindly follow instructions below to proceed:\r\n\r\n" +
                "Enter 1 to Retreive Flows\r\n" +
                "Enter 2 to Retrieve Flows by User ID\r\n" +
                "Enter 3 to Update Flow\r\n" +               
                "Enter 4 to Repeat" +
                "Enter 5 to Exit");

                int thisInput = int.Parse(Console.ReadLine());

                if (thisInput == 1)
                {
                    _workflowController.retrieveFlows();
                }
                else if (thisInput == 2)
                {
                    _workflowController.retrieveFlowsbyUserID();
                }
                else if (thisInput == 3)
                {
                    _workflowController.updateWorkflow();
                }
                else if (thisInput == 4)
                {
                    API();
                }
                else if (thisInput == 5)
                {
                    Environment.Exit(00003);
                }
                
            }
            else if (userInput == 4)
            {
                Console.WriteLine("Kindly follow instructions below to proceed:\r\n\r\n" +
                "Enter 1 to Prieview Applications\r\n" +
                "Enter 2 to Preview Specific Applications\r\n" +
                "Enter 3 to to Repeat\r\n" +
                "Enter 4 to Exit");

                int thisInput = int.Parse(Console.ReadLine());

                if (thisInput == 1)
                {
                    _previewController.applicationPreview();
                }
                else if (thisInput == 2)
                {
                    _previewController.applicationPreviewByUserID();
                }
                else if (thisInput == 3)
                {
                    API();
                }
                else if (thisInput == 4)
                {
                    Environment.Exit(00004);
                }
            }
            else if(userInput == 5)
            {
                API();
            }else if (userInput == 6)
            {
                Environment.Exit(000);
            }
            else
            {
                Console.WriteLine("Kindly Provide a Valid Input");
                userInput = int.Parse(Console.ReadLine());
            }
           
            Console.ReadKey(true);
        }
    }
}
