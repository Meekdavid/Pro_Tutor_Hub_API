using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIAccessProDependencies.Helpers.Common
{
    public class Utils
    {
        /***** STATUS CODES *****/
        public const string StatusCode_Success = "00";
        public const string StatusCode_UserAccountNotFound = "01";
        public const string StatusCode_TokenNullValue = "02";
        public const string StatusCode_BadRequest = "03";
        public const string StatusCode_Unauthorized = "04";
        public const string StatusCode_PartialContent = "05";
        public const string StatusCode_Failure = "06";
        public const string StatusCode_DatabaseConnectionTimeout = "07";
        public const string StatusCode_StoredProcedureError = "08";
        public const string StatusCode_ExceptionError = "09";
        public const string StatusCode_DatabaseConnectionError = "10";

        /***** STATUS MESSAGES *****/
        public const string StatusMessage_Success = "Request Successful.";
        public const string StatusMessage_Failure = "Request Failed";
        public const string StatusMessage_InvalidPhone = "Invalid Phome Number";
        public const string StatusMessage_UnknownError = "Unknown Error Occured while Performing this Action.";
        public const string StatusMessage_BadRequest = "Required request parameter is Invalid / Missing";
        public const string StatusMessage_DatabaseConnectionTimeout = "Database Connection Timeout";
        public const string StatusMessage_ExceptionError = "An Exception Occured";
        public const string StatusMessage_DatabaseConnectionError = "Database Connection Error";
        public const string StatusMessage_RecordNotFound = "Record Not Found";
        public const string StatusMessage_RecordAlreadyExists = "Record Already Exists";
        public const string StatusMessage_ProgramUnavilable = "No Program Found on the Database";        
        public const string StatusMessage_PreviewUnavilable = "No Preview Found on the Database";        
        public const string StatusMessage_WorkflowUnavilable = "No Workflow Found on the Database";        
        public const string StatusMessage_ApplicationFormUnavilable = "No Application Form Found on the Database";        
        

        /***** LOG TYPES *****/
        public enum LogType
        {
            /// <summary>
            /// Log Message in Debug Level
            /// </summary>
            [Description("Log Message in Debug Level")]
            LOG_DEBUG = 1,
            /// <summary>
            /// Log Message in Information Level
            /// </summary>
            [Description("Log Message in Information Level")]
            LOG_INFORMATION = 2,
            /// <summary>
            /// Log Message in Error Level
            /// </summary>
            [Description("Log Message in Error Level")]
            LOG_ERROR = 3
        }

        //APPLICATION HTTP STATUS CODES
        public const int HttpStatusCode_Ok = StatusCodes.Status200OK;
        public const int HttpStatusCode_BadRequest = StatusCodes.Status400BadRequest;
    }
}
