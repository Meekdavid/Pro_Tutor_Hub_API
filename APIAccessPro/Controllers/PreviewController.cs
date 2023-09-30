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
    public class PreviewController : Controller
    {
        private string className = string.Empty;
        private readonly IInputValidation _checkInputSafety;
        private readonly IPreview _previewAccess;
        public PreviewController(IInputValidation checkInputSafety, IPreview previewAccess)
        {
            className = GetType().Name;
            _checkInputSafety = checkInputSafety;
            _previewAccess = previewAccess;
        }

        [ProducesResponseType(typeof(PreviewResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NotSuccessfulResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(NotSuccessfulResponse), StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [HttpGet("Application Preview")]
        public async Task<ActionResult<PreviewResponse>> ApplicationPreview()
        {
            string methodName = "ApplicationPreview", classAndMethodName = $"{className}.{methodName}";
            var logs = new List<Log>();
            PreviewResponse previewResponse = new PreviewResponse();
            var returnHttpStatusCode = Utils.HttpStatusCode_Ok;
            var logBuilder = new StringBuilder($"--------------{classAndMethodName}--------START--------").AppendLine();
            logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Request Received to Preview Application").AppendLine();
            Console.WriteLine($"Current Request is: {methodName}");

            string sqlCosmosQuery = "Select * from c";

            try
            {
                var response = await _previewAccess.ApplicationPreviewAsnyc(sqlCosmosQuery);
                response.Logs.AddToLogs(ref logs);
                logBuilder.ToString().AddToLogs(ref logs);
                logBuilder.Clear();

                if (response.success ?? true)
                {
                    previewResponse.ResponseCode = Utils.StatusCode_Success;
                    previewResponse.ResponseMessage = response._message;
                    previewResponse.PreviewDTO = response.objectValue;
                }
                else
                {
                    previewResponse.ResponseCode = Utils.StatusCode_Failure;
                    previewResponse.ResponseMessage = Utils.StatusMessage_Failure;
                    previewResponse.PreviewDTO = null;
                }
            }
            catch (Exception ex)
            {
                //ON EXCEPTION STORE THE PREVIOUS LOG
                LogWriter.AddLogAndClearLogBuilderOnException(ref logBuilder, LogType.LOG_DEBUG, ref logs, ex, "Adding Form Exception");

                logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Error Encountered while Adding the Form").AppendLine();
                logBuilder.ToString().AddToLogs(ref logs);

                previewResponse.ResponseCode = Utils.StatusCode_ExceptionError;
                previewResponse.ResponseMessage = Utils.StatusMessage_UnknownError;
                previewResponse.PreviewDTO = null;
            }
            finally
            {
                Console.WriteLine($"Result Gotten from the Request: {methodName} is {JsonConvert.SerializeObject(previewResponse)}");
                logBuilder.AppendLine($"--------------{classAndMethodName}--------END--------").AppendLine();
                logBuilder.ToString().AddToLogs(ref logs);
                logBuilder.Clear();
            }
            Task.Run(() => LogWriter.WriteLog(logs));//A separate thread to write logs to file
            return StatusCode(returnHttpStatusCode, previewResponse);
        }

        [ProducesResponseType(typeof(PreviewResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NotSuccessfulResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(NotSuccessfulResponse), StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [HttpGet]
        [Route("Single Application Preview")]
        public async Task<ActionResult<PreviewResponse>> ApplicationPreviewByUserID(string programId)
        {
            string methodName = "ApplicationPreviewByUserID", classAndMethodName = $"{className}.{methodName}";
            var logs = new List<Log>();
            PreviewResponse previewResponse = new PreviewResponse();
            var returnHttpStatusCode = Utils.HttpStatusCode_Ok;
            var logBuilder = new StringBuilder($"--------------{classAndMethodName}--------START--------").AppendLine();
            logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} About Retreiving Preview with programId: {programId}").AppendLine();
            Console.WriteLine($"Current Request is: {methodName}");

            string sqlCosmosQuery = $"Select * from c WHERE c.programId = \"{programId}\"";

            try
            {
                var response = await _previewAccess.ApplicationPreviewAsnyc(sqlCosmosQuery);
                response.Logs.AddToLogs(ref logs);
                logBuilder.ToString().AddToLogs(ref logs);
                logBuilder.Clear();

                if (response.success ?? true)
                {
                    previewResponse.ResponseCode = Utils.StatusCode_Success;
                    previewResponse.ResponseMessage = response._message;
                    previewResponse.PreviewDTO = response.objectValue;
                }
                else
                {
                    previewResponse.ResponseCode = Utils.StatusCode_Failure;
                    previewResponse.ResponseMessage = response._message;
                    previewResponse.PreviewDTO = null;
                }
            }
            catch ( Exception ex )
            {
                //ON EXCEPTION STORE THE PREVIOUS LOG
                LogWriter.AddLogAndClearLogBuilderOnException(ref logBuilder, LogType.LOG_DEBUG, ref logs, ex, "Adding Form Exception");

                logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Error Encountered while Adding the Form").AppendLine();
                logBuilder.ToString().AddToLogs(ref logs);

                previewResponse.ResponseCode = Utils.StatusCode_ExceptionError;
                previewResponse.ResponseMessage = Utils.StatusMessage_UnknownError;
                previewResponse.PreviewDTO = null;

            }
            finally
            {
                Console.WriteLine($"Result Gotten from the Request: {methodName} is {JsonConvert.SerializeObject(previewResponse)}");
                logBuilder.AppendLine($"--------------{classAndMethodName}--------END--------").AppendLine();
                logBuilder.ToString().AddToLogs(ref logs);
                logBuilder.Clear();
            }

            Task.Run(() => LogWriter.WriteLog(logs));//A separate thread to write logs to file
            return StatusCode(returnHttpStatusCode, previewResponse);
        }
    }
}
