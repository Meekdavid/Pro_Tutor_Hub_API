using APIAccessProDependencies.Helpers.DTOs.Global;
using APIAccessProDependencies.Helpers.DTOs.Models;
using APIAccessProDependencies.Helpers.Extensions;
using APIAccessProDependencies.Helpers.Logger;
using APIAccessProDependencies.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static APIAccessProDependencies.Helpers.Common.Utils;

namespace APIAccessProDependencies.Repositories
{
    public class InputValidation : IInputValidation
    {
        private string className = string.Empty;
        public InputValidation()
        {
            className = GetType().Name;
        }
        public async Task<MethodReturnResponse<string>> ValidateInput(string input)
        {
            string methodName = "ValidateInput", classAndMethodName = $"{className}.{methodName}";
            var logs = new List<Log>();
            var logBuilder = new StringBuilder($"--------------{classAndMethodName}--------START--------").AppendLine();
            logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} About Validating the Input: {input}").AppendLine();
            try
            {
                //XML Injection Checks
                string pattern = @"[<>&'$=]|(\bOR\b)";
                if (Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase))
                {
                    input = Regex.Replace(input, pattern, string.Empty);
                    logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} The Input: {input}, is Okay to Proceed.").AppendLine();
                    return new MethodReturnResponse<string>
                    {
                        Logs = logs,
                        objectValue = input
                    };
                }

                input = Regex.Replace(input, "<.*?>", string.Empty);
                logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} The Input: {input}, is Okay to Proceed.").AppendLine();
            }
            catch(Exception ex)
            {
                //ON EXCEPTION STORE THE PREVIOUS LOG
                LogWriter.AddLogAndClearLogBuilderOnException(ref logBuilder, LogType.LOG_DEBUG, ref logs, ex, "Validate Input Exception");

                logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Error Encountered while Validating Input").AppendLine();
                logBuilder.ToString().AddToLogs(ref logs);

                return new MethodReturnResponse<string>
                {
                    Logs = logs,
                    objectValue = input
                };
            }
            finally
            {
                logBuilder.AppendLine($"--------------{classAndMethodName}--------END--------").AppendLine();
                logBuilder.ToString().AddToLogs(ref logs);
                logBuilder.Clear();
            }

            return new MethodReturnResponse<string>
            {
                Logs = logs,
                objectValue = input
            };
        }

        public async Task<MethodReturnResponse<bool>> ValidatePhone(string phoneNo)
        {
            string methodName = "ValidatePhone", classAndMethodName = $"{className}.{methodName}";
            var logs = new List<Log>();
            var logBuilder = new StringBuilder($"--------------{classAndMethodName}--------START--------").AppendLine();
            logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} About Validating the Phone Number: {phoneNo}").AppendLine();
            try
            {
                if (phoneNo.StartsWith("234"))
                {
                    phoneNo = "0" + phoneNo.Substring(3);
                }

                //XML Injection Checks
                string pattern = @"[<>&'$=]|(\bOR\b)";
                phoneNo = Regex.Replace(phoneNo, pattern, string.Empty);

                var IsGoodNumber = ulong.TryParse(phoneNo, out ulong phoneNumber);
                if (!IsGoodNumber)
                {
                    logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} The Phone Number : {phoneNo}, is Okay to Proceed.").AppendLine();
                    return new MethodReturnResponse<bool>
                    {
                        Logs = logs,
                        objectValue = true
                    };
                }

                logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} The Phone Number : {phoneNo}, is Okay to Proceed.").AppendLine();
            }
            catch (Exception ex)
            {
                //ON EXCEPTION STORE THE PREVIOUS LOG
                LogWriter.AddLogAndClearLogBuilderOnException(ref logBuilder, LogType.LOG_DEBUG, ref logs, ex, "Validate Input Exception");

                logBuilder.AppendLine($"{DateTime.Now:dd-MM-yyyy HH:mm:ss} Error Encountered while Validating Input").AppendLine();
                logBuilder.ToString().AddToLogs(ref logs);

                return new MethodReturnResponse<bool>
                {
                    Logs = logs,
                    objectValue = false
                };
            }
            finally
            {
                logBuilder.AppendLine($"--------------{classAndMethodName}--------END--------").AppendLine();
                logBuilder.ToString().AddToLogs(ref logs);
                logBuilder.Clear();
            }

            return new MethodReturnResponse<bool>
            {
                Logs = logs,
                objectValue = false
            };
        }
        public async void ProcessObjectAgainstInputThreats(object inputToValidate)
        {
            try
            {
                if (inputToValidate == null)
                {
                    return;
                }

                //Clear all Inputs from possible XML Injection
                foreach (var property in inputToValidate.GetType().GetProperties())
                {
                    if (property.PropertyType == typeof(string))
                    {
                        var value = (string)property.GetValue(inputToValidate);
                        if (!string.IsNullOrEmpty(value))
                        {
                            // Call the XML Injection Check Method
                            var newValue = await ValidateInput(value);
                            property.SetValue(inputToValidate, newValue.objectValue);
                        }
                    }
                    //else if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                    //{
                    //    ProcessObjectAgainstInputThreats(property.GetValue(inputToValidate));
                    //}
                }
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}
