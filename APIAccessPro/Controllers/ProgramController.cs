using APIAccessProDependencies.Helpers.Common;
using APIAccessProDependencies.Helpers.DTOs;
using APIAccessProDependencies.Helpers.DTOs.Global;
using APIAccessProDependencies.Helpers.DTOs.Models.ApplicationForm;
using APIAccessProDependencies.Helpers.DTOs.Models.Preview;
using APIAccessProDependencies.Helpers.DTOs.Models.Program;
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
    public class ProgramController : Controller
    {
        private string className = string.Empty;
        private readonly IInputValidation _checkInputSafety;
        private readonly IProgram _programAccess;
        public ProgramController(IInputValidation checkInputSafety, IProgram programAccess)
        {
            className = GetType().Name;
            _checkInputSafety = checkInputSafety;
            _programAccess = programAccess;
        }

        [ProducesResponseType(typeof(ProgramResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NotSuccessfulResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(NotSuccessfulResponse), StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [HttpGet("Retreive Programs")]
        public async Task<ActionResult<ProgramResponse>> RetrievePrograms()
        {
            string methodName = "RetrievePrograms", classAndMethodName = $"{className}.{methodName}";
            var logs = new List<Log>();
            ProgramResponse programResponse = new ProgramResponse();
            var returnHttpStatusCode = Utils.HttpStatusCode_Ok;
            var logBuilder = new StringBuilder($"--------------{classAndMethodName}--------START--------").AppendLine();
            logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} About Retreiving all Program from the Database").AppendLine();
            Console.WriteLine($"Current Request is: {methodName}");

            string sqlCosmosQuery = "Select * from c";
                        
            try
            {

                var response = await _programAccess.RetrieveProgramsAsync(sqlCosmosQuery);
                response.Logs.AddToLogs(ref logs);
                logBuilder.ToString().AddToLogs(ref logs);
                logBuilder.Clear();

                if (response.success ?? true)
                {
                    programResponse.ResponseCode = Utils.StatusCode_Success;
                    programResponse.ResponseMessage = response._message;
                    programResponse.ProgramDTO = response.objectValue;
                }
                else
                {
                    programResponse.ResponseCode = Utils.StatusCode_Failure;
                    programResponse.ResponseMessage = Utils.StatusMessage_Failure;
                    programResponse.ProgramDTO = null;
                }
            }
            catch (Exception ex)
            {
                //ON EXCEPTION STORE THE PREVIOUS LOG
                LogWriter.AddLogAndClearLogBuilderOnException(ref logBuilder, LogType.LOG_DEBUG, ref logs, ex, "Adding Form Exception");

                logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Error Encountered while Adding the Form").AppendLine();
                logBuilder.ToString().AddToLogs(ref logs);

                programResponse.ResponseCode = Utils.StatusCode_ExceptionError;
                programResponse.ResponseMessage = Utils.StatusMessage_UnknownError;
                programResponse.ProgramDTO = null;
            }
            finally
            {
                Console.WriteLine($"Result Gotten from the Request: {methodName} is {JsonConvert.SerializeObject(programResponse)}");
                logBuilder.AppendLine($"--------------{classAndMethodName}--------END--------").AppendLine();
                logBuilder.ToString().AddToLogs(ref logs);
                logBuilder.Clear();
            }

            Task.Run(() => LogWriter.WriteLog(logs));//A separate thread to write logs to file
            return StatusCode(returnHttpStatusCode, programResponse);
        }

        [ProducesResponseType(typeof(ProgramResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NotSuccessfulResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(NotSuccessfulResponse), StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [HttpGet]
        [Route("Single Program")]
        public async Task<ActionResult<ProgramResponse>> RetrieveProgramsByUserID(string programId)
        {
            string methodName = "RetrieveProgramsByUserID", classAndMethodName = $"{className}.{methodName}";
            var logs = new List<Log>();
            ProgramResponse programResponse = new ProgramResponse();
            var returnHttpStatusCode = Utils.HttpStatusCode_Ok;
            var logBuilder = new StringBuilder($"--------------{classAndMethodName}--------START--------").AppendLine();
            logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} About Retreiving Program by ID with programID: {programId}").AppendLine();
            Console.WriteLine($"Current Request is: {methodName}");

            string sqlCosmosQuery = $"Select * from c WHERE c.programId = \"{programId}\"";
            
            try
            {

                var response = await _programAccess.RetrieveProgramsAsync(sqlCosmosQuery);
                response.Logs.AddToLogs(ref logs);
                logBuilder.ToString().AddToLogs(ref logs);
                logBuilder.Clear();

                if (response.success ?? true)
                {
                    programResponse.ResponseCode = Utils.StatusCode_Success;
                    programResponse.ResponseMessage = response._message;
                    programResponse.ProgramDTO = response.objectValue;
                }
                else
                {
                    programResponse.ResponseCode = Utils.StatusCode_Failure;
                    programResponse.ResponseMessage = Utils.StatusMessage_Failure;
                    programResponse.ProgramDTO = null;
                }
            }
            catch ( Exception ex )
            {
                //ON EXCEPTION STORE THE PREVIOUS LOG
                LogWriter.AddLogAndClearLogBuilderOnException(ref logBuilder, LogType.LOG_DEBUG, ref logs, ex, "Adding Form Exception");

                logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Error Encountered while Adding the Form").AppendLine();
                logBuilder.ToString().AddToLogs(ref logs);

                programResponse.ResponseCode = Utils.StatusCode_ExceptionError;
                programResponse.ResponseMessage = Utils.StatusMessage_UnknownError;
                programResponse.ProgramDTO = null;
            }
            finally
            {
                Console.WriteLine($"Result Gotten from the Request: {methodName} is {JsonConvert.SerializeObject(programResponse)}");
                logBuilder.AppendLine($"--------------{classAndMethodName}--------END--------").AppendLine();
                logBuilder.ToString().AddToLogs(ref logs);
                logBuilder.Clear();
            }

            Task.Run(() => LogWriter.WriteLog(logs));//A separate thread to write logs to file
            return StatusCode(returnHttpStatusCode, programResponse);
        }

        [ProducesResponseType(typeof(ProgramResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NotSuccessfulResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(NotSuccessfulResponse), StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [HttpPost("Add Program")]
        public async Task<ActionResult<ProgramResponse>> AddPrograms([FromBody] ProgramDTO ProgramToUpdate)
        {
            string methodName = "AddPrograms", classAndMethodName = $"{className}.{methodName}";
            var logs = new List<Log>();
            ProgramResponse programResponse = new ProgramResponse();
            var returnHttpStatusCode = Utils.HttpStatusCode_Ok;
            var logBuilder = new StringBuilder($"--------------{classAndMethodName}--------START--------").AppendLine();
            logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} About Adding a new Program to the Database").AppendLine();
            Console.WriteLine($"Current Request is: {methodName}");

            try
            {

                var response = await _programAccess.AddProgramAsync(ProgramToUpdate);
                response.Logs.AddToLogs(ref logs);
                logBuilder.ToString().AddToLogs(ref logs);
                logBuilder.Clear();

                if (response.success ?? true)
                {
                    programResponse.ResponseCode = Utils.StatusCode_Success;
                    programResponse.ResponseMessage = response._message;
                    programResponse.ProgramDTO = response.objectValue;
                }
                else
                {
                    programResponse.ResponseCode = Utils.StatusCode_Failure;
                    programResponse.ResponseMessage = response._message;
                    programResponse.ProgramDTO = null;
                }
            }
            catch ( Exception ex )
            {
                //ON EXCEPTION STORE THE PREVIOUS LOG
                LogWriter.AddLogAndClearLogBuilderOnException(ref logBuilder, LogType.LOG_DEBUG, ref logs, ex, "Adding Form Exception");

                logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Error Encountered while Adding the Form").AppendLine();
                logBuilder.ToString().AddToLogs(ref logs);

                programResponse.ResponseCode = Utils.StatusCode_ExceptionError;
                programResponse.ResponseMessage = Utils.StatusMessage_UnknownError;
                programResponse.ProgramDTO = null;
            }
            finally
            {
                Console.WriteLine($"Result Gotten from the Request: {methodName} is {JsonConvert.SerializeObject(programResponse)}");
                logBuilder.AppendLine($"--------------{classAndMethodName}--------END--------").AppendLine();
                logBuilder.ToString().AddToLogs(ref logs);
                logBuilder.Clear();
            }

            Task.Run(() => LogWriter.WriteLog(logs));//A separate thread to write logs to file
            return StatusCode(returnHttpStatusCode, programResponse);
        }

        [ProducesResponseType(typeof(ProgramResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NotSuccessfulResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(NotSuccessfulResponse), StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [HttpPut("Update Program")]
        public async Task<ActionResult<ProgramResponse>> UpdateProgram([FromBody] ProgramDTO ProgramToUpdate)
        {
            string methodName = "UpdateProgram", classAndMethodName = $"{className}.{methodName}";
            var logs = new List<Log>();
            ProgramResponse programResponse = new ProgramResponse();
            var returnHttpStatusCode = Utils.HttpStatusCode_Ok;
            var logBuilder = new StringBuilder($"--------------{classAndMethodName}--------START--------").AppendLine();
            logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Request Received to Update Program to Database is: {JsonConvert.SerializeObject(ProgramToUpdate)}").AppendLine();
            Console.WriteLine($"Current Request is: {methodName}");


            try
            {

                var response = await _programAccess.UpdateProgramAsync(ProgramToUpdate);
                response.Logs.AddToLogs(ref logs);
                logBuilder.ToString().AddToLogs(ref logs);
                logBuilder.Clear();

                if (response.success ?? true)
                {
                    programResponse.ResponseCode = Utils.StatusCode_Success;
                    programResponse.ResponseMessage = response._message;
                    programResponse.ProgramDTO = response.objectValue;
                }
                else
                {
                    programResponse.ResponseCode = Utils.StatusCode_Failure;
                    programResponse.ResponseMessage = response._message;
                    programResponse.ProgramDTO = null;
                }
            }
            catch ( Exception ex )
            {
                //ON EXCEPTION STORE THE PREVIOUS LOG
                LogWriter.AddLogAndClearLogBuilderOnException(ref logBuilder, LogType.LOG_DEBUG, ref logs, ex, "Adding Form Exception");

                logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Error Encountered while Adding the Form").AppendLine();
                logBuilder.ToString().AddToLogs(ref logs);

                programResponse.ResponseCode = Utils.StatusCode_ExceptionError;
                programResponse.ResponseMessage = Utils.StatusMessage_UnknownError;
                programResponse.ProgramDTO = null;
            }
            finally
            {
                Console.WriteLine($"Result Gotten from the Request: {methodName} is {JsonConvert.SerializeObject(programResponse)}");
                logBuilder.AppendLine($"--------------{classAndMethodName}--------END--------").AppendLine();
                logBuilder.ToString().AddToLogs(ref logs);
                logBuilder.Clear();
            }

            Task.Run(() => LogWriter.WriteLog(logs));//A separate thread to write logs to file
            return StatusCode(returnHttpStatusCode, programResponse);
        }
    }
}
