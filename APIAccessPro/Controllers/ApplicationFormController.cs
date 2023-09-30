using APIAccessProDependencies.Helpers.Common;
using APIAccessProDependencies.Helpers.DTOs;
using APIAccessProDependencies.Helpers.DTOs.Global;
using APIAccessProDependencies.Helpers.DTOs.Models;
using APIAccessProDependencies.Helpers.DTOs.Models.ApplicationForm;
using APIAccessProDependencies.Helpers.DTOs.Models.Preview;
using APIAccessProDependencies.Helpers.DTOs.Models.Program;
using APIAccessProDependencies.Helpers.DTOs.Models.WorkFlow;
using APIAccessProDependencies.Helpers.Extensions;
using APIAccessProDependencies.Helpers.Logger;
using APIAccessProDependencies.Interfaces;
using APIAccessProDependencies.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using System.Text;
using static APIAccessProDependencies.Helpers.Common.Utils;

namespace APIAccessPro.Controllers
{
    [Route("api")]
    [ApiController]
    public class ApplicationFormController : Controller
    {
        private string className = string.Empty;
        private readonly IInputValidation _checkInputSafety;
        private readonly IApplicationForm _formAccess;
        public ApplicationFormController(IApplicationForm formAccess, IInputValidation checkInputSafety)
        {
            className = GetType().Name;
            _formAccess = formAccess;
            _checkInputSafety = checkInputSafety;
        }

        [ProducesResponseType(typeof(ApplicationFormResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NotSuccessfulResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(NotSuccessfulResponse), StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [HttpGet("Retreive Forms")]
        public async Task<ActionResult<ApplicationFormResponse>> RetrieveForms()
        {
            string methodName = "AddFormAsync", classAndMethodName = $"{className}.{methodName}";
            var logs = new List<Log>();
            ApplicationFormResponse applicationForm = new ApplicationFormResponse();
            var returnHttpStatusCode = Utils.HttpStatusCode_Ok;
            var logBuilder = new StringBuilder($"--------------{classAndMethodName}--------START--------").AppendLine();
            logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} About to Rereive Forms from the Database").AppendLine();
            Console.WriteLine($"Current Request is: {methodName}");

            string sqlCosmosQuery = "Select * from c";

            try
            {
                var response = await _formAccess.RetrieveFormsAsync(sqlCosmosQuery);
                response.Logs.AddToLogs(ref logs);
                logBuilder.ToString().AddToLogs(ref logs);
                logBuilder.Clear();

                if (response.success ?? true)
                {
                    applicationForm.ResponseCode = Utils.StatusCode_Success;
                    applicationForm.ResponseMessage = response._message;
                    applicationForm.ApplicationFormDTO = response.objectValue;
                }
                else
                {
                    applicationForm.ResponseCode = Utils.StatusCode_Failure;
                    applicationForm.ResponseMessage = Utils.StatusMessage_Failure;
                    applicationForm.ApplicationFormDTO = null;
                }
            }
            catch (Exception ex)
            {
                //ON EXCEPTION STORE THE PREVIOUS LOG
                LogWriter.AddLogAndClearLogBuilderOnException(ref logBuilder, LogType.LOG_DEBUG, ref logs, ex, "Adding Form Exception");

                logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Error Encountered while Adding the Form").AppendLine();
                logBuilder.ToString().AddToLogs(ref logs);

                applicationForm.ResponseCode = Utils.StatusCode_ExceptionError;
                applicationForm.ResponseMessage = Utils.StatusMessage_UnknownError;
                applicationForm.ApplicationFormDTO = null;
            }
            finally
            {
                Console.WriteLine($"Result Gotten from the Request: {methodName} is {JsonConvert.SerializeObject(applicationForm)}");
                logBuilder.AppendLine($"--------------{classAndMethodName}--------END--------").AppendLine();
                logBuilder.ToString().AddToLogs(ref logs);
                logBuilder.Clear();
            }

            Task.Run(() => LogWriter.WriteLog(logs));//A separate thread to write logs to file
            return StatusCode(returnHttpStatusCode, applicationForm);
        }

        [ProducesResponseType(typeof(ApplicationFormResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NotSuccessfulResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(NotSuccessfulResponse), StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [HttpGet]
        [Route("Single Form")]
        public async Task<ActionResult<ApplicationFormResponse>> RetrieveFormsbyUserID(string programId)
        {
            string methodName = "RetrieveFormsbyUserID", classAndMethodName = $"{className}.{methodName}";
            var logs = new List<Log>();
            ApplicationFormResponse applicationForm = new ApplicationFormResponse();
            var returnHttpStatusCode = Utils.HttpStatusCode_Ok;
            var logBuilder = new StringBuilder($"--------------{classAndMethodName}--------START--------").AppendLine();
            logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} About Retreiving Forms by programId").AppendLine();
            Console.WriteLine($"Current Request is: {methodName}");

            var sqlCosmosQuery = $"Select * from c WHERE c.programId = \"{programId}\"";

            try
            {

                var response = await _formAccess.RetrieveFormsAsync(sqlCosmosQuery);
                response.Logs.AddToLogs(ref logs);
                logBuilder.ToString().AddToLogs(ref logs);
                logBuilder.Clear();

                if (response.success ?? true)
                {
                    applicationForm.ResponseCode = Utils.StatusCode_Success;
                    applicationForm.ResponseMessage = response._message;
                    applicationForm.ApplicationFormDTO = response.objectValue;
                }
                else
                {
                    applicationForm.ResponseCode = Utils.StatusCode_Failure;
                    applicationForm.ResponseMessage = Utils.StatusMessage_Failure;
                    applicationForm.ApplicationFormDTO = null;
                }
            }
            catch (Exception ex)
            {
                //ON EXCEPTION STORE THE PREVIOUS LOG
                LogWriter.AddLogAndClearLogBuilderOnException(ref logBuilder, LogType.LOG_DEBUG, ref logs, ex, "Adding Form Exception");

                logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Error Encountered while Adding the Form").AppendLine();
                logBuilder.ToString().AddToLogs(ref logs);

                applicationForm.ResponseCode = Utils.StatusCode_ExceptionError;
                applicationForm.ResponseMessage = Utils.StatusMessage_UnknownError;
                applicationForm.ApplicationFormDTO = null;
            }
            finally
            {
                Console.WriteLine($"Result Gotten from the Request: {methodName} is {JsonConvert.SerializeObject(applicationForm)}");
                logBuilder.AppendLine($"--------------{classAndMethodName}--------END--------").AppendLine();
                logBuilder.ToString().AddToLogs(ref logs);
                logBuilder.Clear();
            }

            Task.Run(() => LogWriter.WriteLog(logs));//A separate thread to write logs to file
            return StatusCode(returnHttpStatusCode, applicationForm);
        }


        [ProducesResponseType(typeof(ApplicationFormResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NotSuccessfulResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(NotSuccessfulResponse), StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [HttpPut("Update Forms")]
        public async Task<ActionResult<ApplicationFormResponse>> UpdateForms([FromBody] ApplicationFormDTO newForm)
        {
            string methodName = "UpdateForms", classAndMethodName = $"{className}.{methodName}";
            var logs = new List<Log>();
            ApplicationFormResponse applicationForm = new ApplicationFormResponse();
            var returnHttpStatusCode = Utils.HttpStatusCode_Ok;
            var logBuilder = new StringBuilder($"--------------{classAndMethodName}--------START--------").AppendLine();
            logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Form Received to Add to Database is: {JsonConvert.SerializeObject(newForm)}").AppendLine();
            Console.WriteLine($"Current Request is: {methodName}");

            try
            {
                var validPhone = await _checkInputSafety.ValidatePhone(newForm.personal_Information.phone);
                if (validPhone.success ?? true)
                {
                    //Remove Unwanted Characters from Input that can cause Threats
                    _checkInputSafety.ProcessObjectAgainstInputThreats(newForm);

                    var response = await _formAccess.UpdateFormAsync(newForm);
                    response.Logs.AddToLogs(ref logs);
                    logBuilder.ToString().AddToLogs(ref logs);
                    logBuilder.Clear();

                    if (response.success ?? true)
                    {
                        applicationForm.ResponseCode = Utils.StatusCode_Success;
                        applicationForm.ResponseMessage = response._message;
                        applicationForm.ApplicationFormDTO = response.objectValue;
                    }
                    else
                    {
                        applicationForm.ResponseCode = Utils.StatusCode_Failure;
                        applicationForm.ResponseMessage = response._message;
                        applicationForm.ApplicationFormDTO = null;
                    }
                }
                else
                {
                    applicationForm.ResponseCode = Utils.StatusCode_PartialContent;
                    applicationForm.ResponseMessage = Utils.StatusMessage_InvalidPhone;
                    applicationForm.ApplicationFormDTO = null;
                }

            }
            catch ( Exception ex )
            {
                //ON EXCEPTION STORE THE PREVIOUS LOG
                LogWriter.AddLogAndClearLogBuilderOnException(ref logBuilder, LogType.LOG_DEBUG, ref logs, ex, "Adding Form Exception");

                logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Error Encountered while Adding the Form").AppendLine();
                logBuilder.ToString().AddToLogs(ref logs);

                applicationForm.ResponseCode = Utils.StatusCode_ExceptionError;
                applicationForm.ResponseMessage = Utils.StatusMessage_UnknownError;
                applicationForm.ApplicationFormDTO = null;
            }
            finally
            {
                Console.WriteLine($"Result Gotten from the Request: {methodName} is {JsonConvert.SerializeObject(applicationForm)}");
                logBuilder.AppendLine($"--------------{classAndMethodName}--------END--------").AppendLine();
                logBuilder.ToString().AddToLogs(ref logs);
                logBuilder.Clear();
            }

            Task.Run(() => LogWriter.WriteLog(logs));//A separate thread to write logs to file
            return StatusCode(returnHttpStatusCode, applicationForm);
        }

        //[ProducesResponseType(typeof(ApplicationFormResponse), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(NotSuccessfulResponse), StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(typeof(NotSuccessfulResponse), StatusCodes.Status500InternalServerError)]
        //[Consumes("application/json")]
        //[HttpPost("Delete Forms")]
        //public async Task<ActionResult<ApplicationFormResponse>> DeleteForms(string id, string userID)
        //{
        //    string methodName = "DeleteForms", classAndMethodName = $"{className}.{methodName}";
        //    var logs = new List<Log>();
        //    ApplicationFormResponse applicationForm = new ApplicationFormResponse();
        //    var returnHttpStatusCode = Utils.HttpStatusCode_Ok;
        //    var logBuilder = new StringBuilder($"--------------{classAndMethodName}--------START--------").AppendLine();
        //    logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Form Received to Delete on Database with the UserID: {userID}").AppendLine();
        //    Console.WriteLine($"Current Request is: {methodName}");            

        //    try
        //    {
        //        //Remove Unwanted Characters from Input that can cause Threats
        //        _checkInputSafety.ProcessObjectAgainstInputThreats(id);
        //        _checkInputSafety.ProcessObjectAgainstInputThreats(userID);


        //        var response = await _formAccess.DeleteFormAsync(id, userID);
        //        response.Logs.AddToLogs(ref logs);
        //        logBuilder.ToString().AddToLogs(ref logs);
        //        logBuilder.Clear();

        //        if (response.success ?? true)
        //        {
        //            applicationForm.ResponseCode = Utils.StatusCode_Success;
        //            applicationForm.ResponseMessage = Utils.StatusMessage_Success;
        //            applicationForm.ApplicationFormDTO = response.objectValue;
        //        }
        //        else
        //        {
        //            applicationForm.ResponseCode = Utils.StatusCode_Failure;
        //            applicationForm.ResponseMessage = Utils.StatusMessage_Failure;
        //            applicationForm.ApplicationFormDTO = null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //ON EXCEPTION STORE THE PREVIOUS LOG
        //        LogWriter.AddLogAndClearLogBuilderOnException(ref logBuilder, LogType.LOG_DEBUG, ref logs, ex, "Adding Form Exception");

        //        logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Error Encountered while Adding the Form").AppendLine();
        //        logBuilder.ToString().AddToLogs(ref logs);

        //        applicationForm.ResponseCode = Utils.StatusCode_ExceptionError;
        //        applicationForm.ResponseMessage = Utils.StatusMessage_UnknownError;
        //        applicationForm.ApplicationFormDTO = null;
        //    }
        //    finally
        //    {
        //        Console.WriteLine($"Current Request is: {methodName}");
        //        Console.WriteLine($"Result Gotten from the Request: {methodName} is {JsonConvert.SerializeObject(applicationForm)}");
        //        logBuilder.AppendLine($"--------------{classAndMethodName}--------END--------").AppendLine();
        //        logBuilder.ToString().AddToLogs(ref logs);
        //        logBuilder.Clear();
        //    }

        //    Task.Run(() => LogWriter.WriteLog(logs));//A separate thread to write logs to file
        //    return StatusCode(returnHttpStatusCode, applicationForm);
        //}

        [ProducesResponseType(typeof(ApplicationFormResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(NotSuccessfulResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(NotSuccessfulResponse), StatusCodes.Status500InternalServerError)]
        [Consumes("application/json")]
        [HttpPut("Amend Question")]
        public async Task<ActionResult<ApplicationFormResponse>> AmendAddedQuestions([FromBody] AddQuestion arrayToDelete)
        {
            string methodName = "DeleteAddedQuestions", classAndMethodName = $"{className}.{methodName}";
            var logs = new List<Log>();
            var returnHttpStatusCode = Utils.HttpStatusCode_Ok;
            ApplicationFormResponse applicationFormResponse = new ApplicationFormResponse();
            var logBuilder = new StringBuilder($"--------------{classAndMethodName}--------START--------").AppendLine();
            logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} About Deleting the following Array of Questions: {JsonConvert.SerializeObject(arrayToDelete)}").AppendLine();

            //var sqlCosmosQuery = $"UPDATE c SET c.Add_a_Question= ARRAY_REMOVE(c.Add_a_Question, {arrayToDelete}) WHERE c.id = {ids.documentID}";

            try
            {                
                var response = await _formAccess.AmendFormAsync(arrayToDelete);
                response.Logs.AddToLogs(ref logs);
                logBuilder.ToString().AddToLogs(ref logs);
                logBuilder.Clear();

                if (response.success ?? true)
                {
                    applicationFormResponse.ResponseCode = Utils.StatusCode_Success;
                    applicationFormResponse.ResponseMessage = response._message;
                    applicationFormResponse.ApplicationFormDTO = response.objectValue;
                }
                else
                {
                    applicationFormResponse.ResponseCode = Utils.StatusCode_Failure;
                    applicationFormResponse.ResponseMessage = response._message;
                    applicationFormResponse.ApplicationFormDTO = null;
                }
            }
            catch ( Exception ex )
            {
                //ON EXCEPTION STORE THE PREVIOUS LOG
                LogWriter.AddLogAndClearLogBuilderOnException(ref logBuilder, LogType.LOG_DEBUG, ref logs, ex, "Adding Form Exception");

                logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Error Encountered while Adding the Form").AppendLine();
                logBuilder.ToString().AddToLogs(ref logs);

                applicationFormResponse.ResponseCode = Utils.StatusCode_ExceptionError;
                applicationFormResponse.ResponseMessage = Utils.StatusMessage_UnknownError;
                applicationFormResponse.ApplicationFormDTO = null;
            }
            finally
            {
                Console.WriteLine($"Current Request is: {methodName}");
                Console.WriteLine($"Result Gotten from the Request: {methodName} is {JsonConvert.SerializeObject(applicationFormResponse)}");
                logBuilder.AppendLine($"--------------{classAndMethodName}--------END--------").AppendLine();
                logBuilder.ToString().AddToLogs(ref logs);
                logBuilder.Clear();
            }

            Task.Run(() => LogWriter.WriteLog(logs));//A separate thread to write logs to file
            return StatusCode(returnHttpStatusCode, applicationFormResponse);

        }
    }
}
