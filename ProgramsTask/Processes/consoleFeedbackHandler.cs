using Newtonsoft.Json;
using ProgramsTask.Helpers;
using ProgramsTask.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgramsTask.Helpers;
using Microsoft.Extensions.Logging;
using ProgramsTask.Repositories;

namespace ProgramsTask.Processes
{
    public class consoleFeedbackHandler
    {
        private readonly ILogger<consoleFeedbackHandler> _logger;
        public consoleFeedbackHandler(ILogger<consoleFeedbackHandler> logger)
        {
            _logger = logger;
        }

        public void feedback(string method, List<applicationFormDTO> result)
        {
            string thisMethod = "processFormFeedback";

            _logger.LogInformation($"\r\n\r\n**********Switched From {method} to {thisMethod}**********");
            try
            {      
                
                _logger.LogInformation($"The following\r\n {JsonConvert.SerializeObject(result)}\r\n has been collected from {method}");
                if (!(result == null))
                {
                    _logger.LogInformation($"The result is not null and is about to be returned to the client");
                    Console.WriteLine(JsonConvert.SerializeObject((new communicationModels.APIResponse
                    {
                        responseCode = 200,
                        responseMessage = "Success",
                        responseMethod = method,
                        objectValue = result

                    })));
                    _logger.LogInformation($"Result successfully displayed to the client");
                }

                else
                {
                    Console.WriteLine(JsonConvert.SerializeObject((new communicationModels.NotSuccessfulResponse
                    {
                        StatusCode = "500",
                        StatusMessage = "An Error Occured, Please Try Again"
                    })));
                    _logger.LogInformation($"Bad Response was receieved, and Client has been told to retry");
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"{thisMethod} Exited with Error: {ex}");
                Console.WriteLine(JsonConvert.SerializeObject((new communicationModels.NotSuccessfulResponse
                {
                    StatusCode = "500",
                    StatusMessage = "An Error Occured, Please Try Again"
                })));
            }
            _logger.LogInformation($"**********{thisMethod} Completed Execution, Switching Back to {method}**********\r\n\r\n\r\n");
        }


        public void formFeedback(string method, applicationFormDTO result)
        {
            string thisMethod = "processFormFeedback";


            _logger.LogInformation($"\r\n\r\n**********Switched From {method} to {thisMethod}**********");
            try
            {

                _logger.LogInformation($"The following\r\n {JsonConvert.SerializeObject(result)}\r\n has been collected from {method}");
                if (!(result == null))
                {
                    _logger.LogInformation($"The result is not null and is about to be returned to the client");
                    Console.WriteLine(JsonConvert.SerializeObject((new communicationModels.APIResponse
                    {
                        responseCode = 200,
                        responseMessage = "Success",
                        responseMethod = method,
                        objectValue = null

                    })));
                    _logger.LogInformation($"Result successfully displayed to the client");
                }

                else
                {
                    Console.WriteLine(JsonConvert.SerializeObject((new communicationModels.NotSuccessfulResponse
                    {
                        StatusCode = "500",
                        StatusMessage = "An Error Occured, Please Try Again"
                    })));
                    _logger.LogInformation($"Bad Response was receieved, and Client has been told to retry");

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{thisMethod} Exited with Error: {ex}");
                Console.WriteLine(JsonConvert.SerializeObject((new communicationModels.NotSuccessfulResponse
                {
                    StatusCode = "500",
                    StatusMessage = "An Error Occured, Please Try Again"
                })));
            }
            _logger.LogInformation($"**********{thisMethod} Completed Execution, Switching Back to {method}**********\r\n\r\n\r\n");
        }

        public void previewfeedback(string method, List<previewDTO> result)
        {
            string thisMethod = "processPreviewFeedback";

            _logger.LogInformation($"\r\n\r\n**********Switched From {method} to {thisMethod}**********");
            try
            {

                _logger.LogInformation($"The following\r\n {JsonConvert.SerializeObject(result)}\r\n has been collected from {method}");
                if (!(result == null))
                {
                    _logger.LogInformation($"The result is not null and is about to be returned to the client");
                    Console.WriteLine(JsonConvert.SerializeObject((new communicationModels.PreviewAPIResponse
                    {
                        responseCode = 200,
                        responseMessage = "Success",
                        responseMethod = method,
                        objectValue = result

                    })));
                    _logger.LogInformation($"Result successfully displayed to the client");
                }

                else
                {
                    Console.WriteLine(JsonConvert.SerializeObject((new communicationModels.NotSuccessfulResponse
                    {
                        StatusCode = "500",
                        StatusMessage = "An Error Occured, Please Try Again"
                    })));
                    _logger.LogInformation($"Bad Response was receieved, and Client has been told to retry");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{thisMethod} Exited with Error: {ex}");
                Console.WriteLine(JsonConvert.SerializeObject((new communicationModels.NotSuccessfulResponse
                {
                    StatusCode = "500",
                    StatusMessage = "An Error Occured, Please Try Again"
                })));
            }
            _logger.LogInformation($"**********{thisMethod} Completed Execution, Switching Back to {method}**********\r\n\r\n\r\n");

        }

        public void programfeedback(string method, List<programDTO> result)
        {
            string thisMethod = "processProgramFeedback";

            _logger.LogInformation($"\r\n\r\n**********Switched From {method} to {thisMethod}**********");
            try
            {

                _logger.LogInformation($"\r\n\r\n**********Switched From {method} to {thisMethod}**********");

                _logger.LogInformation($"The following\r\n {JsonConvert.SerializeObject(result)}\r\n has been collected from {method}");
                if (!(result == null))
                {
                    _logger.LogInformation($"The result is not null and is about to be returned to the client");
                    Console.WriteLine(JsonConvert.SerializeObject((new communicationModels.ProgramAPIResponse
                    {
                        responseCode = 200,
                        responseMessage = "Success",
                        responseMethod = method,
                        objectValue = result

                    })));
                    _logger.LogInformation($"Result successfully displayed to the client");
                }

                else
                {
                    Console.WriteLine(JsonConvert.SerializeObject((new communicationModels.NotSuccessfulResponse
                    {
                        StatusCode = "500",
                        StatusMessage = "An Error Occured, Please Try Again"
                    })));
                    _logger.LogInformation($"Bad Response was receieved, and Client has been told to retry");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{thisMethod} Exited with Error: {ex}");
                Console.WriteLine(JsonConvert.SerializeObject((new communicationModels.NotSuccessfulResponse
                {
                    StatusCode = "500",
                    StatusMessage = "An Error Occured, Please Try Again"
                })));
            }
            _logger.LogInformation($"**********{thisMethod} Completed Execution, Switching Back to {method}**********\r\n\r\n\r\n");
        }

        public void addProgramfeedback(string method, programDTO result)
        {
            string thisMethod = "processProgramFeedback";

            _logger.LogInformation($"\r\n\r\n**********Switched From {method} to {thisMethod}**********");
            try
            {

                _logger.LogInformation($"\r\n\r\n**********Switched From {method} to {thisMethod}**********");

                _logger.LogInformation($"The following\r\n {JsonConvert.SerializeObject(result)}\r\n has been collected from {method}");
                if (!(result == null))
                {
                    _logger.LogInformation($"The result is not null and is about to be returned to the client");
                    Console.WriteLine(JsonConvert.SerializeObject((new communicationModels.ProgramAPIResponse
                    {
                        responseCode = 200,
                        responseMessage = "Success",
                        responseMethod = method,
                        objectValue = null

                    })));
                    _logger.LogInformation($"Result successfully displayed to the client");
                }

                else
                {
                    Console.WriteLine(JsonConvert.SerializeObject((new communicationModels.NotSuccessfulResponse
                    {
                        StatusCode = "500",
                        StatusMessage = "An Error Occured, Please Try Again"
                    })));
                    _logger.LogInformation($"Bad Response was receieved, and Client has been told to retry");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{thisMethod} Exited with Error: {ex}");
                Console.WriteLine(JsonConvert.SerializeObject((new communicationModels.NotSuccessfulResponse
                {
                    StatusCode = "500",
                    StatusMessage = "An Error Occured, Please Try Again"
                })));
            }
            _logger.LogInformation($"**********{thisMethod} Completed Execution, Switching Back to {method}**********\r\n\r\n\r\n");
        }

        public void workflowfeedback(string method, List<workflowDTO> result)
        {
            string thisMethod = "processWorkflowFeedback";

            _logger.LogInformation($"\r\n\r\n**********Switched From {method} to {thisMethod}**********");
            try
            {

                _logger.LogInformation($"\r\n\r\n**********Switched From {method} to {thisMethod}**********");

                _logger.LogInformation($"The following\r\n {JsonConvert.SerializeObject(result)}\r\n has been collected from {method}");
                if (!(result == null))
                {
                    _logger.LogInformation($"The result is not null and is about to be returned to the client");
                    Console.WriteLine(JsonConvert.SerializeObject((new communicationModels.WorkflowAPIResponse
                    {
                        responseCode = 200,
                        responseMessage = "Success",
                        responseMethod = method,
                        objectValue = result

                    })));
                    _logger.LogInformation($"Result successfully displayed to the client");
                }

                else
                {
                    Console.WriteLine(JsonConvert.SerializeObject((new communicationModels.NotSuccessfulResponse
                    {
                        StatusCode = "500",
                        StatusMessage = "An Error Occured, Please Try Again"
                    })));
                    _logger.LogInformation($"Bad Response was receieved, and Client has been told to retry");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{thisMethod} Exited with Error: {ex}");
                Console.WriteLine(JsonConvert.SerializeObject((new communicationModels.NotSuccessfulResponse
                {
                    StatusCode = "500",
                    StatusMessage = "An Error Occured, Please Try Again"
                })));
            }
            _logger.LogInformation($"**********{thisMethod} Completed Execution, Switching Back to {method}**********\r\n\r\n\r\n");
        }

        public void updateWorkflowfeedback(string method, workflowDTO result)
        {
            string thisMethod = "processWorkflowUpdateFeedback";

            _logger.LogInformation($"\r\n\r\n**********Switched From {method} to {thisMethod}**********");
            try
            {
                _logger.LogInformation($"\r\n\r\n**********Switched From {method} to {thisMethod}**********");

                _logger.LogInformation($"The following\r\n {JsonConvert.SerializeObject(result)}\r\n has been collected from {method}");
                if (!(result == null))
                {
                    _logger.LogInformation($"The result is not null and is about to be returned to the client");
                    Console.WriteLine(JsonConvert.SerializeObject((new communicationModels.WorkflowAPIResponse
                    {
                        responseCode = 200,
                        responseMessage = "Success",
                        responseMethod = method,
                        objectValue = null

                    })));
                    _logger.LogInformation($"Result successfully displayed to the client");
                }

                else
                {
                    Console.WriteLine(JsonConvert.SerializeObject((new communicationModels.NotSuccessfulResponse
                    {
                        StatusCode = "500",
                        StatusMessage = "An Error Occured, Please Try Again"
                    })));
                    _logger.LogInformation($"Bad Response was receieved, and Client has been told to retry");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{thisMethod} Exited with Error: {ex}");
                Console.WriteLine(JsonConvert.SerializeObject((new communicationModels.NotSuccessfulResponse
                {
                    StatusCode = "500",
                    StatusMessage = "An Error Occured, Please Try Again"
                })));
            }
            _logger.LogInformation($"**********{thisMethod} Completed Execution, Switching Back to {method}**********\r\n\r\n\r\n");
        }
    }
}
