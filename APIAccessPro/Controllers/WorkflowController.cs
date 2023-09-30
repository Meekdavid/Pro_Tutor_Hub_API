using APIAccessProDependencies.Helpers.Common;
using APIAccessProDependencies.Helpers.DTOs;
using APIAccessProDependencies.Helpers.DTOs.Global;
using APIAccessProDependencies.Helpers.DTOs.Models.ApplicationForm;
using APIAccessProDependencies.Helpers.DTOs.Models.Preview;
using APIAccessProDependencies.Helpers.DTOs.Models.Program;
using APIAccessProDependencies.Helpers.DTOs.Models.WorkFlow;
using APIAccessProDependencies.Helpers.Extensions;
using APIAccessProDependencies.Helpers.Logger;
using APIAccessProDependencies.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using static APIAccessProDependencies.Helpers.Common.Utils;

namespace APIAccessPro.Controllers
{
    [Route("api")]
    [ApiController]
    public class WorkflowController : Controller
    {
        private string className = string.Empty;
        private readonly IWorkflow _workflowAccess;
        private readonly IInputValidation _checkInputSafety;
        public WorkflowController(IWorkflow workflowAccess, IInputValidation checkInputSafety)
        {
            className = GetType().Name;
            _workflowAccess = workflowAccess;
            _checkInputSafety = checkInputSafety;
        }

        [ProducesResponseType(typeof(WorkflowResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NotSuccessfulResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(NotSuccessfulResponse), StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [HttpGet("Retreive all Workflows")]
        public async Task<ActionResult<WorkflowResponse>> RetrieveFlows()
        {
            string methodName = "RetrieveFlows", classAndMethodName = $"{className}.{methodName}";
            var logs = new List<Log>();
            var returnHttpStatusCode = Utils.HttpStatusCode_Ok;
            WorkflowResponse flowResponse = new WorkflowResponse();
            var logBuilder = new StringBuilder($"--------------{classAndMethodName}--------START--------").AppendLine();
            logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} About to Retreive Available Flows from the Database").AppendLine();
            Console.WriteLine($"Current Request is: {methodName}");
            string sqlCosmosQuery = "Select * from c";

            try
            {
                var response = await _workflowAccess.RetrieveWorkflowsAsync(sqlCosmosQuery);
                response.Logs.AddToLogs(ref logs);
                logBuilder.ToString().AddToLogs(ref logs);
                logBuilder.Clear();

                if (response.success ?? true)
                {
                    flowResponse.ResponseCode = Utils.StatusCode_Success;
                    flowResponse.ResponseMessage = response._message;
                    flowResponse.Workflow = response.objectValue;
                }
                else
                {
                    flowResponse.ResponseCode = Utils.StatusCode_Success;
                    flowResponse.ResponseMessage = Utils.StatusMessage_Failure;
                    flowResponse.Workflow = null;
                }
            }
            catch( Exception ex )
            {
                //ON EXCEPTION STORE THE PREVIOUS LOG
                LogWriter.AddLogAndClearLogBuilderOnException(ref logBuilder, LogType.LOG_DEBUG, ref logs, ex, "Adding Form Exception");

                logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Error Encountered while Adding the Form").AppendLine();
                logBuilder.ToString().AddToLogs(ref logs);

                flowResponse.ResponseCode = Utils.StatusCode_ExceptionError;
                flowResponse.ResponseMessage = Utils.StatusMessage_UnknownError;
                flowResponse.Workflow = null;
            }
            finally
            {
                Console.WriteLine($"Result Gotten from the Request: {methodName} is {JsonConvert.SerializeObject(flowResponse)}");
                logBuilder.AppendLine($"--------------{classAndMethodName}--------END--------").AppendLine();
                logBuilder.ToString().AddToLogs(ref logs);
                logBuilder.Clear();
            }
            Task.Run(() => LogWriter.WriteLog(logs));//A separate thread to write logs to file
            return StatusCode(returnHttpStatusCode, flowResponse);
        }


        [ProducesResponseType(typeof(WorkflowResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NotSuccessfulResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(NotSuccessfulResponse), StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [HttpGet("Single Flow")]
        public async Task<ActionResult<WorkflowResponse>> RetrieveFlowsbyUserID(string programId)
        {
            string methodName = "RetrieveFlowsbyUserID", classAndMethodName = $"{className}.{methodName}";
            var logs = new List<Log>();
            var returnHttpStatusCode = Utils.HttpStatusCode_Ok;
            WorkflowResponse flowResponse = new WorkflowResponse();
            var logBuilder = new StringBuilder($"--------------{classAndMethodName}--------START--------").AppendLine();
            logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} About Retreiving Form with ProgramID: {programId}").AppendLine();
            Console.WriteLine($"Current Request is: {methodName}");
            
            var sqlCosmosQuery = $"Select * from c WHERE c.programId = \"{programId}\"";
            
            try
            {
                var response = await _workflowAccess.RetrieveWorkflowsAsync(sqlCosmosQuery);
                response.Logs.AddToLogs(ref logs);
                logBuilder.ToString().AddToLogs(ref logs);
                logBuilder.Clear();

                if (response.success ?? true)
                {                    
                    flowResponse.ResponseCode = Utils.StatusCode_Success;
                    flowResponse.ResponseMessage = response._message;
                    flowResponse.Workflow = response.objectValue;
                }
                else
                {
                    flowResponse.ResponseCode = Utils.StatusCode_Success;
                    flowResponse.ResponseMessage = Utils.StatusMessage_Failure;
                    flowResponse.Workflow = null;
                }
            }
            catch (Exception ex)
            {
                //ON EXCEPTION STORE THE PREVIOUS LOG
                LogWriter.AddLogAndClearLogBuilderOnException(ref logBuilder, LogType.LOG_DEBUG, ref logs, ex, "Adding Form Exception");

                logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Error Encountered while Adding the Form").AppendLine();
                logBuilder.ToString().AddToLogs(ref logs);
                
                flowResponse.ResponseCode = Utils.StatusCode_ExceptionError;
                flowResponse.ResponseMessage = Utils.StatusMessage_UnknownError;
                flowResponse.Workflow = null;
            }
            finally
            {
                Console.WriteLine($"Result Gotten from the Request: {methodName} is {JsonConvert.SerializeObject(flowResponse)}");
                logBuilder.AppendLine($"--------------{classAndMethodName}--------END--------").AppendLine();
                logBuilder.ToString().AddToLogs(ref logs);
                logBuilder.Clear();
            }
            Task.Run(() => LogWriter.WriteLog(logs));//A separate thread to write logs to file
            return StatusCode(returnHttpStatusCode, flowResponse);
        }


        [ProducesResponseType(typeof(WorkflowResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NotSuccessfulResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(NotSuccessfulResponse), StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [HttpPut("Update Workflow")]
        public async Task<ActionResult<WorkflowResponse>> UpdateWorkflow([FromBody] WorkflowDTOHolder WorkflowToUpdateHolder)
        {
            string methodName = "UpdateWorkflow", classAndMethodName = $"{className}.{methodName}";
            var logs = new List<Log>();
            var returnHttpStatusCode = Utils.HttpStatusCode_Ok;
            WorkflowResponse flowResponse = new WorkflowResponse();
            var logBuilder = new StringBuilder($"--------------{classAndMethodName}--------START--------").AppendLine();
            logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Workflow Received to Update on Database is: {JsonConvert.SerializeObject(WorkflowToUpdateHolder)}").AppendLine();
            Console.WriteLine($"Current Request is: {methodName}");            
            
            try
            {
                WorkflowDTO WorkflowToUpdate = new WorkflowDTO
                {
                    id = WorkflowToUpdateHolder.id,
                    userID = WorkflowToUpdateHolder.userID,
                    programId = WorkflowToUpdateHolder.programId,
                    stages = WorkflowToUpdateHolder.stages
                };

                var response = await _workflowAccess.UpdateWorkflowsAsync(WorkflowToUpdate, WorkflowToUpdateHolder.command);
                response.Logs.AddToLogs(ref logs);
                logBuilder.ToString().AddToLogs(ref logs);
                logBuilder.Clear();

                if (response.success ?? true)
                {
                    
                    flowResponse.ResponseCode = Utils.StatusCode_Success;
                    flowResponse.ResponseMessage = response._message;
                    flowResponse.Workflow = response.objectValue;
                }
                else
                {
                    flowResponse.ResponseCode = Utils.StatusCode_Failure;
                    flowResponse.ResponseMessage = response._message;
                    flowResponse.Workflow = null;                    
                }
            }
            catch ( Exception ex )
            {
                //ON EXCEPTION STORE THE PREVIOUS LOG
                LogWriter.AddLogAndClearLogBuilderOnException(ref logBuilder, LogType.LOG_DEBUG, ref logs, ex, "Adding Form Exception");

                logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Error Encountered while Adding the Form").AppendLine();
                logBuilder.ToString().AddToLogs(ref logs);

                flowResponse.ResponseCode = Utils.StatusCode_ExceptionError;
                flowResponse.ResponseMessage = Utils.StatusMessage_UnknownError;
                flowResponse.Workflow = null;                
            }
            finally
            {
                Console.WriteLine($"Result Gotten from the Request: {methodName} is {JsonConvert.SerializeObject(flowResponse)}");
                logBuilder.AppendLine($"--------------{classAndMethodName}--------END--------").AppendLine();
                logBuilder.ToString().AddToLogs(ref logs);
                logBuilder.Clear();
            }

            Task.Run(() => LogWriter.WriteLog(logs));//A separate thread to write logs to file
            return StatusCode(returnHttpStatusCode, flowResponse);
        }
        
    }
}
